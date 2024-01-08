using System;
using Hurtownia.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Postal;

namespace Hurtownia.ViewModels
{
    public class PotwierdzenieZamowieniaEmail : Email
    {
        public string To { get; set; }
            public string From { get; set; }
        public decimal Wartosc { get; set; }
        public int NumerZamowienia { get; set; }
        public string Komentarz { get; set; }
        public List<PozycjaZamowienia> PozycjeZamowienia { get; set; }
    }
}