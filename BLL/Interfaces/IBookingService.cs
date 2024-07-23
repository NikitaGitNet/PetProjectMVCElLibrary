using BLL.Models.DTO.Book;
using BLL.Models.DTO.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    /// <summary>
    /// Интерфейс бизнес-логики взаимодействия с ДТО брони
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Сохранение брони в БД на основе ДТО
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        bool CreateBooking(BookingDTO book);
        /// <summary>
        /// Обновление брони в БД на основе ДТО
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        bool UpdateBooking(BookingDTO book);
        /// <summary>
        /// Получение брони на основе ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BookingDTO?> GetBooking(Guid id);
        /// <summary>
        /// Получение всех броней
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<BookingDTO>> GetAllBookings();
        /// <summary>
        /// Получение броней на основании ИД пользователя, маппинг их в ДТО
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<BookingDTO>> GetBookingByUserIdAsync(string id);
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
        /// <summary>
        /// Создание уникального кода для получение пользователем книги
        /// </summary>
        /// <returns></returns>
        string CreateReceiptCode();
    }
}
