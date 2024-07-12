using BLL.Models.DTO.Author;
using BLL.Models.DTO.Genre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAuthorService
    {
        void CreateAuthor(AuthorDTO book);
        Task<AuthorDTO?> GetAuthor(Guid id);
        Task<AuthorDTO?> GetAuthorByName(string name);
        Task<IEnumerable<AuthorDTO>> GetAllAuthors();
        void DeleteAuthor(Guid bookId);
    }
}
