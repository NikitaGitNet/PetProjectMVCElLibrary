using BLL.Interfaces.DTO;
using BLL.Models.DTO.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.DTO.Author
{
    public class AuthorDTO : IAuthorDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<BookDTO>? Books { get; set; }
    }
}
