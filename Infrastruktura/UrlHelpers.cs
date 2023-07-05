using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hurtownia.Infrastruktura
{
    public static class UrlHelpers
    {
        public static string ObrazkiSciezka(this UrlHelper helper, string nazwaPlikuObrazka)
        {
            var ObrazkiFolder = AppConfig.ObrazkiFolderWzgledny;
            var sciezka = Path.Combine(ObrazkiFolder, nazwaPlikuObrazka);
            var SciezkaBezwzgledna = helper.Content(sciezka);
            return SciezkaBezwzgledna;
        }
    }
}