using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System;

namespace Hurtownia.Models 
{ 

public class Kategoria
    {
        public int KategoriaId { get; set; }
    [Required(ErrorMessage = "Wprowadz nazwe produktu")]
    [StringLength(100)]
    public string NazwaKategorii { get; set; }
        [Required(ErrorMessage = "Wprowadz nazwe produktu")]
        [StringLength(100)]
        public string OpisKategorii { get; set; }

        public virtual ICollection<Produkt> Produkty { get; set; }
    }
}
