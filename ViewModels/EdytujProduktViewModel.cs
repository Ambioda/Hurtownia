using Hurtownia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hurtownia.ViewModels
{
    public class EdytujProduktViewModel
    {
        public Produkt Produkt { get; set; }
        public IEnumerable<Kategoria> Kategorie { get; set; }
        public bool? Potwierdzenie { get; set; }
    }
}