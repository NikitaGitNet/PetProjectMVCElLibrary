using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Interfaces.Repository.Genre
{
    /// <summary>
    /// Интерфейс расширяющий логику взаимодействия с энтити жанра
    /// </summary>
    public interface IGenreRepository : IRepository<DAL.Domain.Entities.Genre>
    {
        /// <summary>
        /// Поиск жанра по названию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<DAL.Domain.Entities.Genre?> GetEntityByNameAsync(string name);
    }
}
