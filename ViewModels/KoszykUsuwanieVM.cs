using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hurtownia.ViewModels
{
    public class KoszykUsuwanieVM
    {
        public decimal KoszykCenaCalkowita { get; set; }
        public int KoszykIloscPozycji { get; set; }
        public int IloscPozycjiUsuwanej { get; set; }
        public int IdPozycjiUsuwanej { get; set; }
    }
}