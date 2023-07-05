using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Hurtownia.Infrastruktura
{
    public class AppConfig
    {
        private static string _ObrazkiFolderWzgledny = ConfigurationManager.AppSettings["ObrazkiFolder"];

            public static string ObrazkiFolderWzgledny
        {
            get
            {
                return _ObrazkiFolderWzgledny;
            }
        }
    }
}