using Hurtownia.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hurtownia.Controllers
{
    public class ProduktyController : Controller
    {
        private ProduktyContext db = new ProduktyContext();
        // GET: Produkty
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Lista(string nazwaKategori, string searchQuery = null)
        {
            var kategoria = db.Kategorie.Include("Produkty").Where(k => k.NazwaKategorii.ToUpper() == nazwaKategori.ToUpper()).Single();
            var produkty = kategoria.Produkty.Where(a => searchQuery == null || a.NazwaProduktu.ToLower().Contains(searchQuery.ToLower()) && !a.Ukryty);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProduktyList", produkty);
            }
            return View(produkty);
        }
        public ActionResult Szczegoly(int Id)
        {
            var produkt = db.Produkty.Find(Id);

            return View(produkt);
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60000)]
        public ActionResult KategorieMenu()
        {
            var kategorie = db.Kategorie.ToList();
            return PartialView("_KategorieMenu",kategorie);
        }

    }
}