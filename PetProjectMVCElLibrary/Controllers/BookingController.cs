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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Service;
using PetProjectMVCElLibrary.Service.Logger;
using PetProjectMVCElLibrary.ViewModel.Authorization;
using PetProjectMVCElLibrary.ViewModel.Book;
using PetProjectMVCElLibrary.ViewModel.Booking;
using System.Security.Claims;

namespace PetProjectMVCElLibrary.Controllers
{
    /// <summary>
    /// Контроллер позволяющий пользователю бронировать книги, просматривать свои брони, отменять свои брони
    /// </summary>
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBookingService bookingService;
        private readonly IBookService bookService;
        private readonly IApplicationUserService applicationUserService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FileLogger> _logger;
        public BookingController(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<FileLogger> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _signInManager = signInManager;
            applicationUserService = new ApplicationUserService(context, mapper, signInManager, userManager);
            bookingService = new BookingService(context, mapper);
            bookService = new BookService(context, mapper);
            _logger = logger;
        }
        /// <summary>
        /// Метод отвечающий за вывод всех броней пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Empty;
            // Проверям авторизован ли пользователь
            // Если авторизован, выполняем логику
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                // Получаем ДТО пользователя
                ApplicationUserDTO? applicationUserDTO = new ApplicationUserDTO();
                try
                {
                    applicationUserDTO = await applicationUserService.GetUser(userId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    throw;
                }
                // Если пользователь не найден
                if (applicationUserDTO == null)
                {
                    // Деавторизуем пользователя
                    await _signInManager.SignOutAsync();
                    // Редиректим на экран авторизации
                    return RedirectToAction(nameof(AccountController.Login), new LoginViewModel());
                }
                // Если пользователь найден
                // Получаем брони пользователя, маппим их во ViewModel
                IEnumerable<BookingViewModel> bookings = new List<BookingViewModel>();
                try
                {
                    bookings = _mapper.Map<IEnumerable<BookingViewModel>>(applicationUserDTO.Bookings);
                }
                catch (Exception ex)
                {
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    throw;
                }
                // Передаем ViewModel в представление, выводим все брони пользователя
                return View("Index", bookings);
            }
            // Если не авторизован, редиректим на экран авторизации
            return RedirectToAction(nameof(AccountController.Login), new LoginViewModel());
        }
        /// <summary>
        /// Метод отвечающий за бронирование книги
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Booking(BookViewModel model) 
        {
            // Проверяем авторизован ли пользователь
            // Если авторизован, выполняем логику
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                // Получаем ДТО пользователя
                ApplicationUserDTO? applicationUserDTO = new ApplicationUserDTO();
                try
                {
                    applicationUserDTO = await applicationUserService.GetUser(userId);
                }
                catch (Exception ex)
                {
                    // Генерим лог
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    // Редиректим на окно авторизации
                    return RedirectToAction(nameof(AccountController.Login), new LoginViewModel());
                }
                // Если пользователь не найден
                if (applicationUserDTO == null)
                {
                    // Деавторизуем пользователя
                    await _signInManager.SignOutAsync();
                    // Редиректим на экран авторизации
                    return RedirectToAction(nameof(AccountController.Login), new LoginViewModel());
                }
                // Получаем ДТО книги
                BookDTO? bookDTO = await bookService.GetBook(model.Id);
                // Если книга не найдена
                if (bookDTO == null)
                {
                    // Редиректим на окно выбора книг, пишем сообщени, что книга не найдена
                    TempData["Message"] = "Не удалось забронировать, книга не найдена!";
                    return RedirectToAction(nameof(BookController.Index));
                }
                // Если книга уже забронирована
                if (bookDTO.IsBooking)
                {
                    BookViewModel bookViewModel = new BookViewModel();
                    try
                    {
                        // Редиректим на страницу книги
                        bookViewModel = _mapper.Map<BookViewModel>(bookDTO);
                        return RedirectToAction(nameof(BookController.ShowCurrentBook), bookViewModel);
                    }
                    catch (Exception ex)
                    {
                        // Генерим лог
                        _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                        // Редиректим на окно выбора книги
                        return RedirectToAction(nameof(BookController.Index));
                    }
                }
                // Если броней у пользователя меньше 5, выполняем логику
                if (applicationUserDTO.Bookings.Count() < 5)
                {
                    // Помечаем, что книга теперь забронирована
                    bookDTO.IsBooking = true;
                    // Создаем ДТО брони, базовое время жизни брони 3 дня
                    BookingDTO bookingDTO = new()
                    {
                        BooksTitle = bookDTO.Title,
                        BookId = model.Id,
                        Email = applicationUserDTO.Email,
                        CreateOn = DateTime.Now,
                        FinishedOn = DateTime.Now.AddDays(3),
                        UserId = userId.ToString(),
                        ReceiptCode = bookingService.CreateReceiptCode()
                    };
                    try
                    {
                        // Сохраняем бронь в БД
                        bookingService.CreateBooking(bookingDTO);
                    }
                    catch (Exception ex)
                    {
                        // Генерим лог
                        _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                        // Редиректим на окно выбора книги
                        return RedirectToAction(nameof(BookController.Index));
                    }
                    try
                    {
                        // Обновляем книгу в БД
                        bookService.CreateBook(bookDTO);
                    }
                    catch (Exception ex)
                    {
                        // Генерим лог
                        _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                        // Редиректим на окно выбора книги
                        return RedirectToAction(nameof(BookController.Index));
                    }
                    return View(new BookingViewModel { BookId = bookingDTO.BookId, CreateOn = bookingDTO.CreateOn, FinishedOn = bookingDTO.FinishedOn, ReceiptCode = bookingDTO.ReceiptCode, Email = bookingDTO.Email, BooksTitle = bookingDTO.BooksTitle });
                }
            }
            // Редиректим в окно броней пользователя, генерим сообщение, что превышен лимит броней
            TempData["Message"] = "Превышен лимит броней, максимум броней на одного пользователя: 5";
            return RedirectToAction(nameof(BookingController.Index));
        }
        /// <summary>
        /// Метод позволяющий пользователю самостоятельно отказаться от брони
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(BookViewModel model)
        {
            Guid userId = Guid.Empty;
            // Если пользователь авторизован, выполняем логику
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                // Получаем ДТО пользователя
                ApplicationUserDTO? applicationUserDTO = new ApplicationUserDTO();
                try
                {
                    applicationUserDTO = await applicationUserService.GetUser(userId);
                }
                catch (Exception ex)
                {
                    // Генерим лог
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    // Редиректим на окно авторизации
                    return RedirectToAction(nameof(AccountController.Login), new LoginViewModel());
                }
                // Если юзерДТО не null, выполняем логику
                if (applicationUserDTO != null)
                {
                    // Маппим брони пользователя во ViewModel
                    IEnumerable<BookingViewModel> bookings = new List<BookingViewModel>();
                    try
                    {
                        bookings = _mapper.Map<IEnumerable<BookingViewModel>>(applicationUserDTO.Bookings);
                    }
                    catch (Exception ex)
                    {
                        // Генерим лог
                        _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                        // Редиректим на окно вывода броней пользователя
                        TempData["Message"] = "Не удалось удалить бронь, произошла ошибка";
                        return RedirectToAction(nameof(BookingController.Index));
                    }
                    // Проверяем является ли model бронью этого пользователя
                    if (bookings.Select(x => x.Id).Contains(model.Id))
                    {
                        try
                        {
                            bookingService.DeleteBooking(model.Id);
                        }
                        catch (Exception ex)
                        {
                            // Генерим лог
                            _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                            // Редиректим на окно вывода броней пользователя
                            TempData["Message"] = "Не удалось удалить бронь, произошла ошибка";
                            return RedirectToAction(nameof(BookingController.Index));
                        }
                    }
                    else
                    {
                        _logger.LogWarning(DateTime.Now + "\r\n" + $"Попытка обмана системы. Id пользователя: {applicationUserDTO.Id}" );
                        // Редиректим на окно вывода броней пользователя
                        TempData["Message"] = "Данная бронь не закреплена за вами, вы не можете ее удалить";
                        return RedirectToAction(nameof(BookingController.Index));
                    }
                    // Редиректим на окно вывода броней пользователя
                    return RedirectToAction(nameof(BookingController.Index));
                }
                // Если юзерДТО null, редиректим на окно авторизации
                return RedirectToAction(nameof(AccountController.Login), new LoginViewModel());
            }
            // Если не авторизован, редиректим на окно авторизации
            return RedirectToAction(nameof(AccountController.Login), new LoginViewModel());
        }
    }
}
