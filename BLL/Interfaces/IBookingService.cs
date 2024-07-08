using BLL.Models.DTO.Book;
using BLL.Models.DTO.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBookingService
    {
        /// <summary>
        /// Сохранение брони в БД на основе ДТО
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        Task CreateBooking(BookingDTO book);
        /// <summary>
        /// Получение брони на основе ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BookingDTO> GetBooking(Guid id);
        /// <summary>
        /// Получение всех броней
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<BookingDTO>> GetAllBookings();
        /// <summary>
        /// Удаление брони из БД на основании Id
        /// </summary>
        /// <param name="id"></param>
        void DeleteBooking(Guid bookId);
        /// <summary>
        /// Удаление брони из БД на основании коллекции ДТО
        /// </summary>
        /// <param name="entityes"></param>
        void DeleteRangeBookings(IEnumerable<BookingDTO> entityes);
    }
}
