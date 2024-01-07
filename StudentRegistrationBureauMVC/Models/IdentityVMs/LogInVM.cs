using System.ComponentModel.DataAnnotations;

namespace StudentRegistrationBureauMVC.Models.IdentityVMs
{
    public class LogInVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
