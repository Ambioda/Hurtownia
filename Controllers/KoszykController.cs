﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hurtownia.Controllers
{
    public class KoszykController : Controller
    {
        // GET: Koszyk
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DodajDoKoszyka(string Id)
        {
            return View();
        }
    }
}