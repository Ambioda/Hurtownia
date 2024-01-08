using Hurtownia.Models;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Hurtownia.ViewModels;
using Hurtownia.App_Start;
using System.Collections.Generic;
using Hurtownia.DAL;
using System.Linq;
using System.Data.Entity;
using System;
using System.Net;
using System.IO;
using Hurtownia.Infrastruktura;
using Postal;

namespace Hurtownia.Controllers
{
    public class ZarzadzanieController : Controller
    {
        private ProduktyContext db = new ProduktyContext();
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            Error
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

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }
            if (User.IsInRole("Admin"))
                ViewBag.UserIsAdmin = true;
            else
                ViewBag.UserIsAdmin = false;

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }

            var model = new ManageCredentialsViewModel
            {
                Message = message,
                DaneUzytkownika = user.DaneUzytkownika
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProfile([Bind(Prefix = "DaneUzytkownika")] DaneUzytkownika daneUzytkownika)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                user.DaneUzytkownika = daneUzytkownika;
                var result = await UserManager.UpdateAsync(user);
                AddErrors(result);
            }

            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword([Bind(Prefix = "ChangePasswordViewModel")] ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);

            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }
            var message = ManageMessageId.ChangePasswordSuccess;
            return RedirectToAction("Index", new { Message = message });

        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("password-error", error);
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }


        public ActionResult HistoriaZamowien()
        {
            bool isAdmin = User.IsInRole("Admin");
            ViewBag.UserIsAdmin = isAdmin;
            IEnumerable<Zamowienie> zamowieniaUzytkownika;

            if (isAdmin)
            {
                zamowieniaUzytkownika = db.Zamowienia.Include("PozycjeZamowienia").OrderByDescending(x => x.DataDodania).ToArray();
            }
            else
            {
                var userId = User.Identity.GetUserId();
                zamowieniaUzytkownika = db.Zamowienia.Where(x => x.UserId == userId).Include("PozycjeZamowienia").OrderByDescending(x => x.DataDodania).ToArray();
            }
            return View(zamowieniaUzytkownika);
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult ZmianaStanuZamowienia(Zamowienie zamowienie)
        {
            Zamowienie zamowienieDoModyfikacji = db.Zamowienia.Find(zamowienie.ZamowienieId);
            zamowienieDoModyfikacji.StanZamowienia = zamowienie.StanZamowienia;

            if (!string.IsNullOrEmpty(zamowienieDoModyfikacji.Email))
            {
                PotwierdzenieZmianyStatusu email = new PotwierdzenieZmianyStatusu
                {
                    To = zamowienieDoModyfikacji.Email,
                    From = "ambioda12@interia.pl",
                    NumerZamowienia = zamowienie.ZamowienieId,
                    StanZamowienia = (int)zamowienie.StanZamowienia
                };
                email.Send();
            }

            db.SaveChanges();
            return RedirectToAction("Index"); 
        }
        [HttpGet]
        public ActionResult Szczegoly(int id)
        {
            var zamowienie = db.Zamowienia
                               .Include(z => z.PozycjeZamowienia.Select(p => p.produkt))
                               .FirstOrDefault(z => z.ZamowienieId == id);

            if (zamowienie == null)
            {
                return HttpNotFound();
            }

            var szczegolyZamowienia = new
            {
                ZamowienieId = zamowienie.ZamowienieId,
                DataDodania = zamowienie.DataDodania,
                Komentarz = zamowienie.Komentarz,
                WartoscZamowienia = zamowienie.WartoscZamowienia,
                PozycjeZamowienia = zamowienie.PozycjeZamowienia.Select(p => new
                {
                    ProduktId = p.ProduktId,
                    NazwaProduktu = p.produkt.NazwaProduktu,
                    Ilosc = p.Ilosc,
                    CenaZakupu = p.CenaZakupu
                })
            };

            return Json(szczegolyZamowienia, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DodajProdukt(int? produktId, bool? potwierdzenie)
        {


            Produkt produkt;
            if (produktId.HasValue)
            {
                ViewBag.EditMode = true;
                produkt = db.Produkty.Find(produktId);
            }
            else
            {
                ViewBag.EditMode = false;
                produkt = new Produkt();
            }
            var result = new EdytujProduktViewModel();
            result.Kategorie = db.Kategorie.ToList();
            result.Produkt = produkt;
            result.Potwierdzenie = potwierdzenie;

            return View(result);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult DodajProdukt(EdytujProduktViewModel model, HttpPostedFileBase file)
        {
            if(model.Produkt.ProduktId > 0)
            {
                db.Entry(model.Produkt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DodajProdukt", new { potwierdzenie = true });
            }
            else
            {
                if (file != null && file.ContentLength > 0)
                {
                    if (ModelState.IsValid)

                    {
                        var fileExt = Path.GetExtension(file.FileName);
                        var filename = Guid.NewGuid() + fileExt;
                        var path = Path.Combine(Server.MapPath(AppConfig.ObrazkiFolderWzgledny), filename);
                        file.SaveAs(path);
                        model.Produkt.NazwaPlikuObrazka = filename;
                        model.Produkt.DataDodania = DateTime.Now;

                        db.Entry(model.Produkt).State = EntityState.Added;
                        db.SaveChanges();
                        return RedirectToAction("DodajProdukt", new { potwierdzenie = true });
                    }
                    else
                    {
                        var kategorie = db.Kategorie.ToList();
                        model.Kategorie = kategorie;
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nie wskazano pliku");
                    var kategorie = db.Kategorie.ToList();
                    model.Kategorie = kategorie;
                    return View(model);
                }
            }
 
        }
    }
}