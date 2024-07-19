using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Interfaces.Repository.Comment
{
    /// <summary>
    /// Интерфейс расширяющий логику взаимодействия с энтити комментария
    /// </summary>
    public interface ICommentRepository : IRepository<DAL.Domain.Entities.Comment>
    {
        /// <summary>
        /// Получение комментариев по ИД книги
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<DAL.Domain.Entities.Comment>> GetEntityesByBookIdAsync(Guid id);
    }
}
