using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hurtownia.Models
{
    public class Zamowienie
    {
        public int ZamowienieId { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "Wprowadz Imie")]
        [StringLength(20)]
        public string Imie { get; set; }
        [Required(ErrorMessage = "Wprowadz Nazwisko")]
        [StringLength(30)]
        public string Nazwisko { get; set; }
        [Required(ErrorMessage = "Wprowadz Ulice")]
        [StringLength(50)]
        public string Adres { get; set; }
        [Required(ErrorMessage = "Wprowadz Miasto")]
        [StringLength(50)]
        public string Miasto { get; set; }

        [Required(ErrorMessage = "Wprowadz Kod Pocztowy")]
        [StringLength(6)]
        public string KodPocztowy { get; set; }
        [RegularExpression(@"(\+\d{2})*[\d\s-]+", ErrorMessage = "Błędny format numeru telefonu")]
        [Required(ErrorMessage = "Wprowadz Telefon")]
        [StringLength(11)]
        public string Telefon { get; set; }
        [Required(ErrorMessage = "Wprowadz E-mail")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage ="Błędny format adresu E-mail")]
        public string Email { get; set; }
        public string Komentarz { get; set; }
        public DateTime DataDodania { get; set; }
        public StanZamowienia StanZamowienia { get; set; }

        public decimal WartoscZamowienia { get; set; }
      public  List<PozycjaZamowienia> PozycjeZamowienia { get; set; }
    }

    public enum StanZamowienia
    {
        Nowe,
        Zrealizowane
    }

}