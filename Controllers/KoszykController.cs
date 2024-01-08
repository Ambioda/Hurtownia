using Hurtownia.App_Start;
using Hurtownia.DAL;
using Hurtownia.Infrastruktura;
using Hurtownia.Models;
using Hurtownia.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult ProduktNiedostepny()
        {
            return View();
        }
        public ActionResult DodajDoKoszyka(int Id, int ilosc)
        {
            var produkt = db.Produkty.FirstOrDefault(p => p.ProduktId == Id);

            if (produkt != null && produkt.DostepnaIlosc >= ilosc)
            {
                koszykMenager.DodajDoKoszyka(Id, ilosc);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Produkt jest niedostępny lub podano zbyt dużą ilość.";
                return RedirectToAction("ProduktNiedostepny");
            }
        }


        public int PobierzIloscElementowKoszyka()
        {
            return koszykMenager.PobierzIloscPozycjiKoszyka();
        }
        
            public ActionResult Podsumowanie()
            {
            var pozycjeKoszyka = koszykMenager.PobierzKoszyk();
            var cenaCalkowita = koszykMenager.PobierzWartoscKoszyka();

            var model = new KoszykViewModel
            {
                PozycjeKoszyka = pozycjeKoszyka,
                CenaCalkowita = cenaCalkowita
            };

            return View(model);
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
        public async Task<ActionResult> Zaplac()
        {
            if (Request.IsAuthenticated)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                var zamowienie = new Zamowienie
                {
                    Imie = user.DaneUzytkownika.Imie,
                    Nazwisko = user.DaneUzytkownika.Nazwisko,
                    Adres = user.DaneUzytkownika.Adres,
                    Miasto = user.DaneUzytkownika.Miasto,
                    KodPocztowy = user.DaneUzytkownika.Kod_Pocztowy,
                    Email = user.DaneUzytkownika.Email,
                    Telefon = user.DaneUzytkownika.Telefon,
                };
            
                return View(zamowienie);
            }
            else
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Zaplac", "Koszyk") });
        }
        [HttpPost]
        public async Task<ActionResult> Zaplac(Zamowienie zamowienieSzczegoly)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var newOrder = koszykMenager.UtworzZamowienie(zamowienieSzczegoly, userId);
                var user = await UserManager.FindByIdAsync(userId);
                TryUpdateModel(user.DaneUzytkownika);
                await UserManager.UpdateAsync(user);
                koszykMenager.PustyKoszyk();
                var zamowienie = db.Zamowienia.Include("PozycjeZamowienia").Include("PozycjeZamowienia.Produkt").SingleOrDefault(o => o.ZamowienieId == newOrder.ZamowienieId);
                PotwierdzenieZamowieniaEmail email = new PotwierdzenieZamowieniaEmail();
                email.To = zamowienie.Email;
                email.From = "ambioda12@interia.pl";
                email.Wartosc = zamowienie.WartoscZamowienia;
                email.NumerZamowienia = zamowienie.ZamowienieId;
                email.PozycjeZamowienia = zamowienie.PozycjeZamowienia;
                email.Komentarz = zamowienie.Komentarz;
                email.Send();
                return RedirectToAction("PotwierdzenieZamowienia");

            }
            else
                return View(zamowienieSzczegoly);
        }
        public ActionResult PotwierdzenieZamowienia()
        {
            return View();
        }


        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}