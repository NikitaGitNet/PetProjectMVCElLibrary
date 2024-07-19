using BLL.Interfaces.DTO;
using BLL.Models.DTO.Book;

namespace BLL.Models.DTO.Genre
{
    /// <summary>
    /// ДТО жанра
    /// </summary>
    public class GenreDTO : IGenreDTO
    {
        /// <summary>
        /// ИД жанра
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Название жанра
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Коллекция ДТО свзанных книг
        /// </summary>
        public IEnumerable<BookDTO>? Books { get; set; }
    }
}
