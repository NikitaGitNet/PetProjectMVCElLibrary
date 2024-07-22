using System.ComponentModel.DataAnnotations;

namespace PetProjectMVCElLibrary.ViewModel.Authorization
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле не заполнено")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Адрес электронной почты")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        [UIHint("password")]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }
        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }
    }
}
