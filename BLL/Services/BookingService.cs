using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Booking;
using DAL.Domain;
using DAL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    internal class BookingService : IBookingService
    {
        private readonly UnitOfWorkRepository Database;
        private readonly IMapper mapper;
        public BookingService(AppDbContext context)
        {
            Database = new UnitOfWorkRepository(context);
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<Booking, BookingDTO>()).CreateMapper();
        }
        public async Task<IEnumerable<BookingDTO>> GetAllBookings()
        {
            IEnumerable<Booking> bookings = await Database.BookingRepository.GetAllEntityesAsync();
            return mapper.Map<IEnumerable<Booking>, IEnumerable<BookingDTO>>(bookings);
        }

        public Task<BookingDTO> GetBooking(Guid id)
        {
            throw new NotImplementedException();
        }
        public Task CreateBooking(BookingDTO book)
        {
            throw new NotImplementedException();
        }

        public void DeleteBooking(Guid bookId)
        {
            throw new NotImplementedException();
        }

        public void DeleteRangeBookings(IEnumerable<BookingDTO> entityes)
        {
            throw new NotImplementedException();
        }

        
    }
}
