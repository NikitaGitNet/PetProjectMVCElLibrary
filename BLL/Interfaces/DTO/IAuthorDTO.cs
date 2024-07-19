using BLL.Models.DTO.Book;

namespace BLL.Interfaces.DTO
{
    /// <summary>
    /// Интерфейс ДТО автора
    /// </summary>
    public interface IAuthorDTO
    {
        /// <summary>
        /// ИД автора
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        string? Name { get; set; }
        /// <summary>
        /// Коллекция ДТО связанных книг
        /// </summary>
        IEnumerable<BookDTO>? Books { get; set; }
    }
}
