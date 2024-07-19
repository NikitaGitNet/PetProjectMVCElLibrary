using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Interfaces.Repository.Book
{
    /// <summary>
    /// Интерфейс расширяющий логику взаимодействия с энтити книги
    /// </summary>
    public interface IBookRepository : IRepository<DAL.Domain.Entities.Book>
    {
        /// <summary>
        /// Получение книг по ИД жанра
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        Task<IEnumerable<DAL.Domain.Entities.Book>> GetEntityesByGenreAsync(Guid genreId);
        /// <summary>
        /// Получение книг по ИД автора
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        Task<IEnumerable<DAL.Domain.Entities.Book>> GetEntityesByAuthorAsync(Guid authorId);
        /// <summary>
        /// Массовое обновление книг
        /// </summary>
        /// <param name="books"></param>
        void UpdateEntityRange(IEnumerable<DAL.Domain.Entities.Book> books);
    }
}
