using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository;
using DAL.Domain.Interfaces.Repository.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUnitOfWork
    {
        IBookRepository BookRepository { get; }
    }
}
