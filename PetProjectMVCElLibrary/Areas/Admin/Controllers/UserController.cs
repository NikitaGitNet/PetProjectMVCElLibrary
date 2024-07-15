using AutoMapper;
using BLL.Interfaces;
using DAL.Domain.Entities;
using DAL.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BLL.Services.ApplicationUser;
using BLL.Models.DTO.ApplicationUser;
using PetProjectMVCElLibrary.ViewModel.User;
using BLL.Services.Book;
using PetProjectMVCElLibrary.ViewModel.Booking;
using PetProjectMVCElLibrary.ViewModel.Comment;

namespace PetProjectMVCElLibrary.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBookService bookService;
        public UserController(AppDbContext context, IMapper mapper, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _applicationUserService = new ApplicationUserService(context, signInManager, mapper);
            bookService = new BookService(context, mapper);
            _signInManager = signInManager;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Show()
        {
            IEnumerable<ApplicationUserDTO> applicationUserDTOs = await _applicationUserService.GetAllUsers();
            return View(_mapper.Map<IEnumerable<ApplicationUserViewModel>>(applicationUserDTOs));
        }
        [HttpPost]
        public async IActionResult ShowCurrentUser(string userId)
        {
            ApplicationUserDTO? applicationUserDTO = await _applicationUserService.GetUser(Guid.Parse(userId));
            if (applicationUserDTO != null)
            {
                //if (applicationUserDTO.Bookings.Any())
                //{
                //    applicationUserViewModel.Bookings = _mapper.Map<IEnumerable<BookingViewModel>>(applicationUserDTO.Bookings);
                //}
                //if (applicationUserDTO.Comments.Any())
                //{
                //    applicationUserViewModel.Comments = _mapper.Map<IEnumerable<CommentViewModel>>(applicationUserDTO.Comments);
                //}
                return RedirectToAction(nameof(UserController.Show));
            }
            return View("~/Views/ErrorPage.cshtml", "Пользователь не найден");
        }
        [HttpPost]
        public IActionResult SearchByEmail(string email)
        {
            if (email != null)
            {
                IEnumerable<ApplicationUser> users = userRepository.GetAll();
                var sortUsers = from user in users where user.Email.ToUpper().Contains(model.UserEmail.ToUpper()) select user;
                List<UserViewModel> userViewModels = new();
                foreach (var user in sortUsers)
                {
                    UserViewModel userViewModel = new()
                    {
                        Email = user.Email,
                        UserName = user.UserName,
                        CreateOn = user.CreateOn,
                        Id = user.Id
                    };
                    userViewModels.Add(userViewModel);
                }
                return View("UsersShow", new UsersListViewModel { Users = userViewModels });
            }
            return RedirectToAction(nameof(UsersShowController.UsersShow), nameof(UsersShowController).CutController());
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UserModel model)
        {
            ApplicationUser user = userRepository.GetById(new Guid(model.Id));
            if (user.Bookings.Any())
            {
                foreach (var booking in user.Bookings)
                {
                    Guid bookId = booking.BookId;
                    Book book = bookRepository.GetById(bookId);
                    book.IsBooking = false;
                    bookRepository.Save(book);
                }
                bookingRepository.DeleteRange(model.Id);
            }
            if (user.Comments.Any())
            {
                commentRepository.DeleteRange(model.Id);
            }
            await userManager.IsLockedOutAsync(user);
            await userManager.DeleteAsync(user);
            return View("Delete");
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserModel model)
        {
            ApplicationUser user = await userManager.FindByIdAsync(model.Id);
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, model.Password);
            await userManager.UpdateAsync(user);
            return View(new UserModel { Password = model.Password });
        }
    }
}
