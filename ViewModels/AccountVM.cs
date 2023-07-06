using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hurtownia.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Prosze wprowadzic e-mail")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Prosze wprowadzic hasło")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamietaj mnie")]
        public bool RememberMe { get; set; }

    }

    public class RegisterVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Prosze wprowadzic hasło")]
        [StringLength(30, ErrorMessage = "{0} musi miec co najmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Prosze wprowadzic potwierdzić hasło")]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź Hasło")]
        [Compare("Password", ErrorMessage =" Hasła nie są identyczne.")]
        public string ConfirmPassword { get; set; }

    }
}