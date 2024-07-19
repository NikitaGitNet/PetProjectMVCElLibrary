using BLL.Models.DTO.Book;

namespace BLL.Interfaces.DTO
{
    /// <summary>
    /// Интерфейс ДТО жанра
    /// </summary>
    public interface IGenreDTO
    {
        /// <summary>
        /// ИД жанра
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// Название жанра
        /// </summary>
        string? Name { get; set; }
        /// <summary>
        /// Коллекция ДТО свзанных книг
        /// </summary>
        IEnumerable<BookDTO>? Books { get; set; }
    }
}
