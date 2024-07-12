using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Booking;
using BLL.Services;
using DAL.Domain;
using DAL.Domain.Entities;
using DAL.Domain.Repository;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.ViewModel.Booking;

namespace PetProjectMVCElLibrary.Areas.Moderator.Controllers
{
    [Area("Moderator")]
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBookingService bookingService;
        private readonly IBookService bookService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BookingController(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            bookingService = new BookingService(context, mapper);
            bookService = new BookService(context, mapper);
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<BookingDTO> bookingDTOs = await bookingService.GetAllBookings();
            return View(_mapper.Map<IEnumerable<BookingViewModel>>(bookingDTOs));
        }
        [HttpGet]
        public async Task<IActionResult> ShowCurrentBooking(Guid id)
        {
            BookingDTO? bookingDTO = await bookingService.GetBooking(id);
            if (bookingDTO != null)
            {
                return View(_mapper.Map<BookingViewModel>(bookingDTO));
            }
            return View("~/Views/ErrorPage.cshtml", "Ошибка! Бронь не найдена");
        }
        [HttpGet]
        public async Task<IActionResult> IssueBooking(Guid id)
        {
            BookingDTO? bookingDTO = await bookingService.GetBooking(id);
            if (bookingDTO != null)
            {
                bookingDTO.IssueBooking = true;
                bookingDTO.FinishedOn = DateTime.Now.AddDays(7);
                bookingService.CreateBooking(bookingDTO);
                return View(_mapper.Map<BookingViewModel>(bookingDTO));
            }
            return View("~/Views/ErrorPage.cshtml", "Ошибка! Бронь не найдена");
        }
        public async Task<IActionResult> Delete(Guid id)
        { 
            BookingDTO? bookingDTO = await bookingService.GetBooking(id);
            if (bookingDTO != null)
            {
                BookDTO bookDTO = await bookService.GetBook(bookingDTO.BookId);
                if (bookDTO != null)
                {
                    bookDTO.IsBooking = false;
                    bookService.CreateBook(bookDTO);
                    bookingService.DeleteBooking(bookingDTO.Id);
                    return RedirectToAction("Index");
                }
                bookingService.DeleteBooking(bookingDTO.Id);
                return RedirectToAction("Index");
            }
            return View("~/Views/ErrorPage.cshtml", "Ошибка! Бронь не найдена");
        }
    }
}
