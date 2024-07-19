using BLL.Models.DTO.Comment;

namespace BLL.Interfaces.DTO
{
    /// <summary>
    /// Интерфейс ДТО книги
    /// </summary>
    public interface IBookDTO
    {
        /// <summary>
        /// ИД книги
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// Название книги
        /// </summary>
        string? Title { get; set; }
        /// <summary>
        /// Подзаголовок
        /// </summary>
        string? SubTitle { get; set; }
        /// <summary>
        /// Наименование автора книги
        /// </summary>
        string? AuthorName { get; set; }
        /// <summary>
        /// Название жанра книги
        /// </summary>
        string? GenreName { get; set; }
        /// <summary>
        /// Путь к обложке книги
        /// </summary>
        string? TitleImagePath { get; set; }
        /// <summary>
        /// Описание книги
        /// </summary>
        string? Text { get; set; }
        /// <summary>
        /// Флаг, забронирована книги или нет
        /// </summary>
        bool IsBooking { get; set; }
        /// <summary>
        /// Дата добавления книги
        /// </summary>
        DateTime DateAdded { get; set; }
        /// <summary>
        /// ИД автора
        /// </summary>
        Guid AuthorId { get; set; }
        /// <summary>
        /// ИД жанра
        /// </summary>
        Guid GenreId { get; set; }
        /// <summary>
        /// Коллекция ДТО связанных комментов
        /// </summary>
        public IEnumerable<CommentDTO> Comments { get; set; }
    }
}
