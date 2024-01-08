using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Postal;
using Hurtownia.Models;

namespace Hurtownia.ViewModels
{
    public class PotwierdzenieZmianyStatusu : Email
    {

            public string To { get; set; }
            public string From { get; set; }
            public int NumerZamowienia { get; set; }
        public int StanZamowienia { get; set; }
        }
    }
