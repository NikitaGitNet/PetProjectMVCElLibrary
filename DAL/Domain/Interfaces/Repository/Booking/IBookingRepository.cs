using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Interfaces.Repository.Booking
{
    /// <summary>
    /// Интерфейс расширяющий логику взаимодействия с энтити брони
    /// </summary>
    public interface IBookingRepository : IRepository<DAL.Domain.Entities.Booking>
    {
        /// <summary>
        /// Поиск броней связанных с пользователем по ИД пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<DAL.Domain.Entities.Booking>> GetBookingByUserIdAsync(string id);
    }
}
