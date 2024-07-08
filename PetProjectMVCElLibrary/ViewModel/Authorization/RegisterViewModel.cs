using System.ComponentModel.DataAnnotations;

namespace PetProjectMVCElLibrary.ViewModel.Authorization
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string? UserName { get; set; }
        [Required]
        [Display(Name = "Адрес электронной почты")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string? PasswordConfirm { get; set; }
        public bool MaxLengthName = false;
        public bool UniqueName = false;
    }
}
