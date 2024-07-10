using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.ApplicationUser;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Booking;
using BLL.Services;
using DAL.Domain;
using DAL.Domain.Entities;
using DAL.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.ViewModel.Book;
using PetProjectMVCElLibrary.ViewModel.Booking;
using PetProjectMVCElLibrary.ViewModel.User;
using System.Security.Claims;

namespace PetProjectMVCElLibrary.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBookingService bookingService;
        private readonly IBookService bookService;
        private readonly IApplicationUserService applicationUserService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BookingController(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _signInManager = signInManager;
            applicationUserService = new ApplicationUserService(context,signInManager, mapper);
            bookingService = new BookingService(context, mapper);
            bookService = new BookService(context, mapper);
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = string.Empty;
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                Claim? claim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null) 
                {
                    userId = claim.Value;
                }
            }
            ApplicationUserDTO applicationUserDTO = await applicationUserService.GetUser(userId);
            if (applicationUserDTO == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            IEnumerable<BookingDTO> bookingDTOs = await bookingService.GetBookingByUserId(userId);
            if (bookingDTOs.Any())
            {
                List<BookingViewModel> bookings = new();
                foreach (var item in applicationUserDTO.Bookings)
                {
                    BookingViewModel booking = new()
                    {
                        IssueBooking = item.IssueBooking,
                        UserEmail = item.UserEmail ?? string.Empty,
                        CreateOn = item.CreateOn,
                        FinishedOn = item.FinishedOn,
                        BookId = item.BookId,
                        BooksTitle = item.BooksTitle ?? string.Empty,
                        Id = item.Id
                    };
                    bookings.Add(booking);
                }
                return View("Show", new ApplicationUserViewModel { Id = applicationUserDTO.Id, UserName = applicationUserDTO.UserName, Bookings = bookings, UserEmail = applicationUserDTO.UserEmail });
            }
            return View("NullPage");
        }
        [HttpPost]
        public async Task<IActionResult> Booking(BookViewModel model) 
        {
            string userId = string.Empty;
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                Claim? claim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    userId = claim.Value;
                }
            }
            ApplicationUserDTO applicationUserDTO = await applicationUserService.GetUser(userId);
            if (applicationUserDTO == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            BookDTO bookDTO = await bookService.GetBook(model.Id);
            if (bookDTO == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (applicationUserDTO.Bookings.Count < 5)
            {
                bookDTO.IsBooking = true;
                BookingDTO bookingDTO = new()
                {
                    BooksTitle = bookDTO.Title,
                    BookId = model.Id,
                    UserEmail = applicationUserDTO.UserEmail,
                    CreateOn = DateTime.Now,
                    FinishedOn = DateTime.Now.AddDays(3),
                    UserId = userId,
                };
                bookingService.CreateBooking(bookingDTO);
                await bookService.CreateBook(bookDTO);
                return View(new BookingViewModel { BookId = bookingDTO.BookId, CreateOn = bookingDTO.CreateOn, FinishedOn = bookingDTO.FinishedOn, Id = bookingDTO.Id, UserEmail = bookingDTO.UserEmail, BooksTitle = bookingDTO.BooksTitle });
            }
            return View("LimitBooking");
        }
        ///// <summary>
        /////     Check user is active
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="idUser">out UserId</param>
        ///// <returns></returns>
        //public static bool IsUserTry(IHttpContextAccessor context, out Guid idUser)
        //{
        //    if (context is null) throw new ArgumentNullException(@"Context is null");
        //    idUser = Guid.Empty;
        //    var currentUser = context.HttpContext?.User;
        //    var user = currentUser?.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier);
        //    if (user != null)
        //    {
        //        if (Guid.TryParse(user.Value, out idUser))
        //            return true;
        //        return false;
        //    }
        //    return false;
        //}
    }
}
