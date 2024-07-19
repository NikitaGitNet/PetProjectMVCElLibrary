using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.ApplicationUser;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Booking;
using BLL.Services;
using BLL.Services.ApplicationUser;
using BLL.Services.Book;
using BLL.Services.Booking;
using DAL.Domain;
using DAL.Domain.Entities;
using DAL.Domain.Repository;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Service;
using PetProjectMVCElLibrary.Service.Logger;
using PetProjectMVCElLibrary.ViewModel.Booking;

namespace PetProjectMVCElLibrary.Areas.Moderator.Controllers
{
    /// <summary>
    /// Контроллер содержаший логику для взаимодействия модератора с бронями
    /// </summary>
    [Area("Moderator")]
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBookingService bookingService;
        private readonly IBookService bookService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FileLogger> _logger;
        private readonly IApplicationUserService _applicationUserService;
        public BookingController(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger<FileLogger> logger, IApplicationUserService applicationUserService)
        {
            _context = context;
            _mapper = mapper;
            bookingService = new BookingService(context, mapper);
            bookService = new BookService(context, mapper);
            _httpContextAccessor = httpContextAccessor;
            _applicationUserService = applicationUserService;
            _logger = logger;
        }
        /// <summary>
        /// Метод вывода всех броней
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Проверям авторизован ли пользователь
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Получаем ДТО пользователя
                    ApplicationUserDTO? userDTO = await _applicationUserService.GetUser(userId);
                    if (userDTO != null)
                    {
                        // Проверяем является ли пользователь модератором
                        if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "moderator"))
                        {
                            // Получаем коллекцию ДТО всех броней, мапим во ViewModel, возвращаем
                            IEnumerable<BookingDTO> bookingDTOs = await bookingService.GetAllBookings();
                            return View(_mapper.Map<IEnumerable<BookingViewModel>>(bookingDTOs));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке вывода броней произошла ошибка!";
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            TempData["Message"] = "При попытке вывода броней произошла ошибка!";
            return RedirectToAction(nameof(HomeController.Index));
        }
        /// <summary>
        /// Метод вывода конктретной брони
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ShowCurrentBooking(Guid id)
        {
            // Проверям авторизован ли пользователь
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Получаем ДТО пользователя
                    ApplicationUserDTO? userDTO = await _applicationUserService.GetUser(userId);
                    if (userDTO != null)
                    {
                        // Проверяем является ли пользователь модератором
                        if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "moderator"))
                        {
                            // Получаем ДТО брони по ИД, мапим во ViewModel, предаем в представление, возвращаем
                            BookingDTO? bookingDTO = await bookingService.GetBooking(id);
                            if (bookingDTO != null)
                            {
                                return View(_mapper.Map<BookingViewModel>(bookingDTO));
                            }
                            TempData["Message"] = "При попытке вывода брони произошла ошибка! Бронь не найдена";
                            return RedirectToAction(nameof(BookingController.Index));
                        }
                    }
                }
                catch (Exception ex) 
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке вывода брони произошла ошибка!";
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            return RedirectToAction(nameof(HomeController.Index));
        }
        /// <summary>
        /// Метод выдачи книги пользователю
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> IssueBooking(Guid id)
        {
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Получаем ДТО пользователя
                    ApplicationUserDTO? userDTO = await _applicationUserService.GetUser(userId);
                    if (userDTO != null)
                    {
                        // Проверяем является ли пользователь модератором
                        if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "moderator"))
                        {
                            BookingDTO? bookingDTO = await bookingService.GetBooking(id);
                            if (bookingDTO != null)
                            {
                                bookingDTO.IssueBooking = true;
                                bookingDTO.FinishedOn = DateTime.Now.AddDays(7);
                                bookingService.CreateBooking(bookingDTO);
                                return View(_mapper.Map<BookingViewModel>(bookingDTO));
                            }
                            TempData["Message"] = "При попытке выдать книгу произошла ошибка! Бронь не найдена";
                            return RedirectToAction(nameof(BookingController.Index));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке выдать книгу произошла ошибка!";
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            TempData["Message"] = "При попытке выдать книгу произошла ошибка!";
            return RedirectToAction(nameof(HomeController.Index));
        }
        /// <summary>
        /// Метод удаления/закрытия брони
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id)
        {
            // Проверям авторизован ли пользователь
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Получаем ДТО пользователя
                    ApplicationUserDTO? userDTO = await _applicationUserService.GetUser(userId);
                    if (userDTO != null)
                    {
                        // Проверяем является ли пользователь модератором
                        if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "moderator"))
                        {
                            // Получаем ДТО брони
                            BookingDTO? bookingDTO = await bookingService.GetBooking(id);
                            if (bookingDTO != null)
                            {
                                // Получаем книгу
                                BookDTO? bookDTO = await bookService.GetBook(bookingDTO.BookId);
                                
                                if (bookDTO != null)
                                {
                                    // Ставим флаг, что книга больше не забронирована, обновляем книгу
                                    bookDTO.IsBooking = false;
                                    bookService.CreateBook(bookDTO);
                                    bookingService.DeleteBooking(bookingDTO.Id);
                                    return RedirectToAction("Index");
                                }
                                bookingService.DeleteBooking(bookingDTO.Id);
                                TempData["Message"] = "Бронь успешно закрыта";
                                return RedirectToAction(nameof(BookingController.Index));
                            }
                            TempData["Message"] = "При попытке закрыть бронь произошла ошибка!";
                            return RedirectToAction(nameof(BookingController.Index));
                        }
                    }
                }
                catch (Exception ex) 
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке выдать книгу произошла ошибка!";
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            return RedirectToAction(nameof(HomeController.Index));
        }
    }
}
