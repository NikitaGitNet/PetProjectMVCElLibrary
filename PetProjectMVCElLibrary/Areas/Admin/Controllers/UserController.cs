using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.ApplicationUser;
using BLL.Services.ApplicationUser;
using BLL.Services.Book;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.ViewModel.User;

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
            _applicationUserService = new ApplicationUserService(context, mapper, signInManager, userManager);
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
        public async Task<IActionResult> ShowCurrentUser(ApplicationUserViewModel applicationUserViewModel)
        {
            ApplicationUserDTO? applicationUserDTO = await _applicationUserService.GetUser(Guid.Parse(applicationUserViewModel.Id ?? ""));
            if (applicationUserDTO != null)
            {
                return View(_mapper.Map<ApplicationUserViewModel>(applicationUserDTO));
            }
            TempData["Result"] = "Пользователь не найден";
            return RedirectToAction(nameof(UserController.Show));
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
        public async Task<IActionResult> ChangePassword(ApplicationUserViewModel applicationUserViewModel)
        {
            bool result = await _applicationUserService.ChangePassword(Guid.Parse(applicationUserViewModel.Id ?? ""), applicationUserViewModel.Password ?? "");
            if (result)
            {
                TempData["Result"] = "Пароль успешно изменен";
                return RedirectToAction(nameof(UserController.ShowCurrentUser), applicationUserViewModel);
            }
            TempData["Result"] = "Неудача, не удалось узменить пароль";
            return RedirectToAction(nameof(UserController.ShowCurrentUser), applicationUserViewModel);
        }
    }
}
