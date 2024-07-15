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
        public async Task<IActionResult> ShowCurrentUser(string userId)
        {
            ApplicationUserDTO? applicationUserDTO = await _applicationUserService.GetUser(Guid.Parse(userId));
            if (applicationUserDTO != null)
            {
                return View(_mapper.Map<ApplicationUserViewModel>(applicationUserDTO));
            }
            return View("~/Views/ErrorPage.cshtml", "Пользователь не найден");
        }
        [HttpPost]
        public async Task<IActionResult> SearchByEmail(string email)
        {
            if (email != null)
            {
                ApplicationUserDTO applicationUserDTO = await _applicationUserService.GetUserByEmail(email);
                if (applicationUserDTO != null) 
                {
                    return View("ShowCurrentUser", _mapper.Map<ApplicationUserViewModel>(applicationUserDTO));
                }
            }
            return View("~/Views/ErrorPage.cshtml", "Пользователь не найден");
        }
        [HttpPost]
        public IActionResult Delete(string userId)
        {
            _applicationUserService.DeleteUser(Guid.Parse(userId));
            return View("Delete");
        }
        [HttpPost]
        public IActionResult ChangePassword(string userId, string password)
        {
            _applicationUserService.ChangePassword(Guid.Parse(userId), password);
            return View();
        }
    }
}
