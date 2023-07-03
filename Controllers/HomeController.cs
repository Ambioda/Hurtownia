using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hurtownia.DAL;
using Hurtownia.Models;

namespace Hurtownia.Controllers
{
    public class HomeController : Controller
    {
        private ProduktyContext db = new ProduktyContext();
        public ActionResult Index()
        {
            var listaKategorii = db.Kategorie.ToList();
            return View();
        }
    }
}