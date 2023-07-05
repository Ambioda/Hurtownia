using Hurtownia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hurtownia.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Kategoria> Kategorie { get; set; }
        public IEnumerable<Produkt> Nowosci { get; set; }
        public IEnumerable<Produkt> BestSellery { get; set; }
    }
}