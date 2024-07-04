using BLL.Interfaces.DTO;
using BLL.Models.DTO.Book;

namespace BLL.Interfaces
{
    public interface IBookService
    {
        Task CreateBook(IBookDTO book);
        Task<BookDTO> GetBook(Guid id);
        Task<IEnumerable<IBookDTO>> GetAllBooks();
    }
}
