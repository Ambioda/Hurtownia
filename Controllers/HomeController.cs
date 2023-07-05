using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hurtownia.DAL;
using Hurtownia.Models;
using Hurtownia.ViewModels;

namespace Hurtownia.Controllers
{
    public class HomeController : Controller
    {
        private ProduktyContext db = new ProduktyContext();
        public ActionResult Index()
        {
            var kategorie = db.Kategorie.ToList();

            var nowosci = db.Produkty.Where(a => !a.Ukryty).OrderByDescending(a => a.DataDodania).Take(3).ToList();

            var bestseller = db.Produkty.Where(a => !a.Ukryty && a.Bestseller).OrderBy(a => Guid.NewGuid()).Take(3).ToList();

            var vm = new HomeViewModel()
            {
                Kategorie = kategorie,
                Nowosci = nowosci,
                BestSellery = bestseller
            };
            return View(vm);
        }
        public ActionResult StronyStatyczne(string nazwa)
        {
            return View(nazwa);
        }
    }
}