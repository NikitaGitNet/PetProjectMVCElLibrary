using BLL.Models.DTO.Book;
using DAL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.DTO
{
    public interface IAuthorDTO
    {
        Guid Id { get; set; }
        string? Name { get; set; }
        IEnumerable<BookDTO>? Books { get; set; }
    }
}
