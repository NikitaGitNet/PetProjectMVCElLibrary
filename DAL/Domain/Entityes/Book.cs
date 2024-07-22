using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Domain.Entities
{
    /// <summary>
    /// Энтити книги
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Название книги
        /// </summary>
        [Required(ErrorMessage = "Заполните название книги")]
        [Display(Name = "Название книги")]
        public string? Title { get; set; }
        /// <summary>
        /// Краткое описание книги
        /// </summary>
        [Display(Name = "Краткое описание книги")]
        public string? SubTitle { get; set; }
        /// <summary>
        /// Флаг - забронирована книга или нет
        /// </summary>
        public bool IsBooking { get; set; }
        /// <summary>
        /// Полное описание книги
        /// </summary>
        [Display(Name = "Полное описание книги")]
        public string? Text { get; set; }
        /// <summary>
        /// Наименование автора книги
        /// </summary>
        [Display(Name = "Автор книги, Заполнить в формате - Фамилия Имя Отчество")]
        public string? AuthorName { get; set; }
        /// <summary>
        /// Внешний ключ автора книги, тип связи один ко многим
        /// </summary>
        [ForeignKey("AuthorId")]
        public Guid AuthorId { get; set; }
        public Author Author { get; set; } = null!;
        /// <summary>
        /// Внешний ключ жанра книги, тип связи один ко многим
        /// </summary>
        [ForeignKey("GenreId")]
        public Guid GenreId { get; set; }
        [Display(Name = "Жанр книги")]
        public string? GenreName { get; set; }
        public Genre Genre { get; set; } = null!;
        /// <summary>
        /// Путь к обложке книги, обложки храняться в ~/wwwroot/images/
        /// </summary>
        public string? TitleImagePath { get; set; }
        /// <summary>
        /// Комменты которые относятся к книге, тип связи, один ко многим
        /// </summary>
        public ICollection<Comment> Comments { get; set; } = null!;
        /// <summary>
        /// Брони книги, тип связи один ко многим.
        /// На текущий момент 1 книга может единовременно иметь только 1 бронь, один ко многим делал с заделом на будующее, если вдруг приспичит добавлять книги не в еденичном экземпляре
        /// </summary>
        public ICollection<Booking> Bookings { get; set; } = null!;
    }
}
