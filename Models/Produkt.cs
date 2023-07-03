using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Hurtownia.Models
{
    public class Produkt
    {
        public int ProduktId { get; set; }
        public int KategoriaId { get; set; }
        [Required(ErrorMessage ="Wprowadz nazwe produktu")]
        [StringLength(100)]
        public string NazwaProduktu { get; set; }
        public DateTime DataDodania { get; set; }
        [StringLength(100)]
        public string NazwaPlikuObrazka { get; set; }
        public string OpisProduktu { get; set; }
        public decimal CenaProduktu { get; set; }
        public bool Bestseller { get; set; }
        public bool Ukryty { get; set; }

        public virtual Kategoria Kategoria { get; set; }
    }
}