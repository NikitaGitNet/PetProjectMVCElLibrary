using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Interfaces.Repository.Book
{
    public interface IBookRepository : IRepository<DAL.Domain.Entities.Book>
    {
        Task<IEnumerable<DAL.Domain.Entities.Book>> GetEntityesByGenreAsync(Guid genreId);
        Task<IEnumerable<DAL.Domain.Entities.Book>> GetEntityesByAuthorAsync(Guid authorId);
        void UpdateEntityRange(IEnumerable<DAL.Domain.Entities.Book> books);
    }
}
