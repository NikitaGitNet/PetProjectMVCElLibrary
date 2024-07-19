using System.ComponentModel.DataAnnotations;

namespace DAL.Domain.Entities
{
    /// <summary>
    /// Энитити текстового поля
    /// Текстовое поле заполняется администратором в админке, служит для вывода какой-либо информации на отдельных страницах сайта
    /// </summary>
    public class TextField
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Кодовое слово, по которому текстовое поле будем доставать из БД
        /// </summary>
        [Required]
        public string? CodeWord { get; set; }
        /// <summary>
        /// Название страницы (заголовок)
        /// </summary>
        [Display(Name = "Название страницы (заголовок)")]
        public string? Title { get; set; }
        /// <summary>
        /// Основная часть страницы - ее содержание
        /// </summary>
        [Display(Name = "Содержание страницы")]
        public string? Text { get; set; }
    }
}
