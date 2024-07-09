using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Interfaces.Repository.Comment
{

    public interface ICommentRepository : IRepository<DAL.Domain.Entities.Comment>
    {
        Task<IEnumerable<DAL.Domain.Entities.Comment>> GetEntityesByBookIdAsync(Guid id);
    }
}
