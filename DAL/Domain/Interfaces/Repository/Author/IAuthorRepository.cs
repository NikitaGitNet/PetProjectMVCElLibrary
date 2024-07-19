using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Interfaces.Repository.Author
{
    /// <summary>
    /// Интерфейс расширяющий логику взаимодействия с энтити автора
    /// </summary>
    public interface IAuthorRepository : IRepository<DAL.Domain.Entities.Author>
    {
        /// <summary>
        /// Метод поиска автора по наименованию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<DAL.Domain.Entities.Author?> GetEntityByNameAsync(string name);
    }
}
