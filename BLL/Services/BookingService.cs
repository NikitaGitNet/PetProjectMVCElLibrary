using AutoMapper;
using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Models.DTO.Booking;
using DAL.Domain;
using DAL.Domain.Entities;

namespace BLL.Services
{
    public class BookingService : IBookingService
    {
        private readonly UnitOfWorkRepository Database;
        private readonly IMapper _mapper;
        public BookingService(AppDbContext context, IMapper mapper)
        {
            Database = new UnitOfWorkRepository(context);
            _mapper = mapper;
        }
        /// <summary>
        /// Получение всех броней, маппинг их в ДТО
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BookingDTO>> GetAllBookings()
        {
            IEnumerable<Booking> bookings = await Database.BookingRepository.GetAllEntityesAsync();
            return _mapper.Map<IEnumerable<BookingDTO>>(bookings);
        }
        /// <summary>
        /// Получение брони по Id, маппинг в ДТО
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task<BookingDTO> GetBooking(Guid id)
        {
            Booking? booking = await Database.BookingRepository.GetEntityByIdAsync(id);
            if (booking != null)
            {
                return _mapper.Map<BookingDTO>(booking);
            }
            throw new ValidationException("Бронь не найдена", "");
        }
        /// <summary>
        /// Получение броней по ИД пользователя, маппинг их в ДТО
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookingDTO>> GetBookingByUserIdAsync(string id)
        {
            IEnumerable<Booking> bookings = await Database.BookingRepository.GetBookingByUserIdAsync(id);
            return _mapper.Map<IEnumerable<BookingDTO>>(bookings);
        }
        /// <summary>
        /// Маппинг брони ДТО в энтити, сохранение энтити в БД
        /// </summary>
        /// <param name="book"></param>
        public void CreateBooking(BookingDTO bookingDTO)
        {
            Booking booking = _mapper.Map<Booking>(bookingDTO);
            Database.BookingRepository.SaveEntity(booking);
        }
        /// <summary>
        /// Удаление брони из БД
        /// </summary>
        /// <param name="bookId"></param>
        public void DeleteBooking(Guid bookId)
        {
            Database.BookingRepository.DeleteEntity(bookId);
        }
        public void DeleteRangeBookings(IEnumerable<BookingDTO> entityes)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Создание уникального кода для получения книги
        /// Создаем гуид, режем последние 28 символов, оставляем первые 8, получаем уникальный код
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string CreateReceiptCode()
        {
            return Guid.NewGuid().ToString().Remove(8, 28).ToUpper();
        }
    }
}
