using BLL.Interfaces.DTO;
using BLL.Models.DTO.Comment;
using System.ComponentModel.DataAnnotations;


namespace BLL.Models.DTO.Book
{
    /// <summary>
    /// ДТО книги
    /// </summary>
    public class BookDTO : IBookDTO
    {
        public BookDTO()
        {
            Comments = new List<CommentDTO>();
        }
        /// <summary>
        /// ИД книги
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Название книги
        /// </summary>
        [Required(ErrorMessage = "Заполните название книги")]
        [Display(Name = "Название книги")]
        public string? Title { get; set; }
        /// <summary>
        /// Подзаголовок
        /// </summary>
        [Display(Name = "Краткое описание книги")]
        public string? SubTitle { get; set; }
        /// <summary>
        /// Полное описание книги
        /// </summary>
        [Display(Name = "Полное описание книги")]
        public string? Text { get; set; }
        /// <summary>
        /// Наименование автора книги
        /// </summary>
        [Display(Name = "Автор")]
        public string? AuthorName { get; set; }
        /// <summary>
        /// Название жанра книги
        /// </summary>
        [Display(Name = "Жанр")]
        public string? GenreName { get; set; }
        /// <summary>
        /// Путь к обложке книги
        /// </summary>
        public string? TitleImagePath { get; set; }
        /// <summary>
        /// ИД автора
        /// </summary>
        public Guid AuthorId { get; set; }
        /// <summary>
        /// ИД жанра
        /// </summary>
        public Guid GenreId { get; set; }
        /// <summary>
        /// Флаг забронированна книга или нет
        /// </summary>
        public bool IsBooking { get; set; }
        /// <summary>
        /// Дата добавления книги
        /// </summary>
        public DateTime DateAdded { get; set; }
        /// <summary>
        /// Коллекция ДТО связанных комментов
        /// </summary>
        public IEnumerable<CommentDTO> Comments { get; set; }
    }
}
