using System.ComponentModel.DataAnnotations;

namespace PetProjectMVCElLibrary.ViewModel.Authorization
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Адрес электронной почты")]
        public string? Email { get; set; }
        [Required]
        [UIHint("password")]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }
        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }
    }
}
