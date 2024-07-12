using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Interfaces.Repository.Author
{
    public interface IAuthorRepository : IRepository<DAL.Domain.Entities.Author>
    {
        Task<DAL.Domain.Entities.Author?> GetEntityByNameAsync(string name);
    }
}
