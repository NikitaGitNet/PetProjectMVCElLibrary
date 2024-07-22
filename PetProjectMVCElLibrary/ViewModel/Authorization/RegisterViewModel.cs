using System.ComponentModel.DataAnnotations;

namespace PetProjectMVCElLibrary.ViewModel.Authorization
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле не заполнено")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Минимум 5 символов")]
        [Display(Name = "Имя пользователя")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Адрес электронной почты")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string? PasswordConfirm { get; set; }
    }
}
