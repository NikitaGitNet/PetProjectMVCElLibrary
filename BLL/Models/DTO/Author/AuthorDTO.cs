using BLL.Interfaces.DTO;
using BLL.Models.DTO.Book;

namespace BLL.Models.DTO.Author
{
    /// <summary>
    /// ДТО автора
    /// </summary>
    public class AuthorDTO : IAuthorDTO
    {
        /// <summary>
        /// ИД автора
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Коллекция ДТО связанных книг
        /// </summary>
        public IEnumerable<BookDTO>? Books { get; set; }
    }
}
