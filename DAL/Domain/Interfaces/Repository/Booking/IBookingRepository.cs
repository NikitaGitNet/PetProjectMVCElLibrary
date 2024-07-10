using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Interfaces.Repository.Booking
{
    public interface IBookingRepository : IRepository<DAL.Domain.Entities.Booking>
    {
        Task<IEnumerable<DAL.Domain.Entities.Booking>> GetBookingByUserIdAsync(string id);
    }
}
