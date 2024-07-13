using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Interfaces.Repository.TextField
{
    public interface ITextFieldRepository : IRepository<DAL.Domain.Entities.TextField>
    {
        Task<DAL.Domain.Entities.TextField?> GetTextFieldByCodeWord(string codeWord);
    }
}
