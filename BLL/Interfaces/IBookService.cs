using BLL.Interfaces.DTO;
using BLL.Models.DTO.Book;

namespace BLL.Interfaces
{
    public interface IBookService
    {
        /// <summary>
        /// Сохранение книги в БД на основе ДТО
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        void CreateBook(BookDTO book);
        /// <summary>
        /// Получение книги на основек ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BookDTO> GetBook(Guid id);
        /// <summary>
        /// Получение всех книг
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<BookDTO>> GetAllBooks();
        /// <summary>
        /// Получение всех книг по конкретному жанру
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        Task<IEnumerable<BookDTO>> GetBookByGenre(Guid genreId);
        /// <summary>
        /// Получение всех книг по конкретному автору
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        Task<IEnumerable<BookDTO>> GetBookByAuthor(Guid authorId);
        /// <summary>
        /// Удаление книги из БД на основании Id
        /// </summary>
        /// <param name="id"></param>
        void DeleteBook(Guid bookId);
        /// <summary>
        /// Удаление entityes из БД на основании коллекции entityes
        /// </summary>
        /// <param name="entityes"></param>
        void DeleteRangeBooks(IEnumerable<BookDTO> entityes);
    }
}
