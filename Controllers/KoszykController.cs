using Hurtownia.DAL;
using Hurtownia.Infrastruktura;
using Hurtownia.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hurtownia.Controllers
{
    public class KoszykController : Controller
    {
        private KoszykMenager koszykMenager;
        private ISessionManager sessionManager { get; set; }
        private ProduktyContext db;

        public KoszykController()
        {
            db = new ProduktyContext();
            sessionManager = new SessionMenager();
            koszykMenager = new KoszykMenager(sessionManager, db);
        }
        // GET: Koszyk
        public ActionResult Index()
        {
            var pozycjeKoszyka = koszykMenager.PobierzKoszyk();
            var cenaCalkowita = koszykMenager.PobierzWartoscKoszyka();

            KoszykViewModel koszykVM = new KoszykViewModel()
            {
                PozycjeKoszyka = pozycjeKoszyka,
                CenaCalkowita = cenaCalkowita
            };

            return View(koszykVM);
        }
        public ActionResult DodajDoKoszyka(int Id)
        {
            koszykMenager.DodajDoKoszyka(Id);

            return RedirectToAction("Index");
        }

        public int PobierzIloscElementowKoszyka()
        {
            return koszykMenager.PobierzIloscPozycjiKoszyka();
        }
        public ActionResult UsunZKoszyka(int produktId)
        {
            int iloscPozycji = koszykMenager.UsunZKoszyka(produktId);
            int iloscPozycjiKoszyka = koszykMenager.PobierzIloscPozycjiKoszyka();
            decimal wartoscKoszyka = koszykMenager.PobierzWartoscKoszyka();

            var result = new KoszykUsuwanieVM
            {
                IdPozycjiUsuwanej = produktId,
                IloscPozycjiUsuwanej = iloscPozycji,
                KoszykCenaCalkowita = wartoscKoszyka,
                KoszykIloscPozycji = iloscPozycjiKoszyka
            };
            return Json(result);
        }
    }
}