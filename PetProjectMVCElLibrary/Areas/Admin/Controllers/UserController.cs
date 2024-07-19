using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.ApplicationUser;
using BLL.Models.DTO.TextField;
using BLL.Services.ApplicationUser;
using BLL.Services.Book;
using BLL.Services.TextField;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Areas.Admin.ViewModel.TextField;
using PetProjectMVCElLibrary.Controllers;
using PetProjectMVCElLibrary.Service;
using PetProjectMVCElLibrary.Service.Logger;
using PetProjectMVCElLibrary.ViewModel.Authorization;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FileLogger> _logger;
        public UserController(AppDbContext context, IMapper mapper, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ILogger<FileLogger> logger)
        {
            _applicationUserService = new ApplicationUserService(context, mapper, signInManager, userManager);
            bookService = new BookService(context, mapper);
            _signInManager = signInManager;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        /// <summary>
        /// Метод вывода всех зарегестрированных пользователей(кроме админа и модератора) на экран
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Show()
        {
            IEnumerable<ApplicationUserDTO> applicationUserDTOs = new List<ApplicationUserDTO>();
            // Проверям авторизован ли пользователь
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Получем на пальзователя ДТО
                    ApplicationUserDTO? userDTO = await _applicationUserService.GetUser(userId);
                    if (userDTO != null)
                    {
                        // Проверяем является ли он админом
                        if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "admin"))
                        {
                            // Мапим во ViewModel, предаем в представление, возвращаем представление
                            applicationUserDTOs = await _applicationUserService.GetAllUsers();
                            return View(_mapper.Map<IEnumerable<ApplicationUserViewModel>>(applicationUserDTOs));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель админа
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке вывода списка пользователей произошла ошибка!";
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            // Редиректим на панель админа
            return RedirectToAction(nameof(HomeController.Index));
        }
        /// <summary>
        /// Метод для вовода подробной информации по конкретному пользователю
        /// </summary>
        /// <param name="applicationUserViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ShowCurrentUser(ApplicationUserViewModel applicationUserViewModel)
        {
            // Проверям авторизован ли пользователь
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Получем на текущего пальзователя ДТО
                    ApplicationUserDTO? userDTO = await _applicationUserService.GetUser(userId);
                    if (userDTO != null)
                    {
                        // Проверяем является ли он админом
                        if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "admin"))
                        {
                            ApplicationUserDTO? applicationUserDTO = await _applicationUserService.GetUser(Guid.Parse(applicationUserViewModel.Id ?? ""));
                            if (applicationUserDTO != null)
                            {
                                return View(_mapper.Map<ApplicationUserViewModel>(applicationUserDTO));
                            }
                            TempData["Result"] = "Пользователь не найден";
                            return RedirectToAction(nameof(UserController.Show));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, на окно вывода всех пользователей
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке получения данных о пользователе произошла ошибка!";
                    return RedirectToAction(nameof(UserController.Show));
                }
            }
            return RedirectToAction(nameof(UserController.Show));
        }
        /// <summary>
        /// Метод поиска пользователя по Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SearchByEmail(string email)
        {
            // Проверям авторизован ли пользователь
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Получем на текущего пальзователя ДТО
                    ApplicationUserDTO? userDTO = await _applicationUserService.GetUser(userId);
                    if (userDTO != null)
                    {
                        if (email != null)
                        {
                            // Ищем пользователя по email, мапим во ViewModel, передаем в предствление, возвращаем представление
                            ApplicationUserDTO? applicationUserDTO = await _applicationUserService.GetUserByEmail(email);
                            if (applicationUserDTO != null)
                            {
                                return View("ShowCurrentUser", _mapper.Map<ApplicationUserViewModel>(applicationUserDTO));
                            }
                        }
                        TempData["Message"] = "Пользователь не найден";
                        return RedirectToAction(nameof(UserController.Show));
                    }
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, на окно вывода всех пользователей
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке получения данных о пользователе произошла ошибка!";
                    return RedirectToAction(nameof(UserController.Show));
                }
            }
            return RedirectToAction(nameof(UserController.Show));
        }
        /// <summary>
        /// Метод удаления пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(string Id)
        {
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    ApplicationUserDTO? userDTO = await _applicationUserService.GetUser(userId);
                    if (userDTO != null)
                    {
                        // Проверяем является ли он админом
                        if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "admin"))
                        {
                            _applicationUserService.DeleteUser(Guid.Parse(Id));
                            return RedirectToAction(nameof(UserController.Show));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, на окно вывода всех пользователей
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При удалении пользователя произошла ошибка!";
                    return RedirectToAction(nameof(UserController.Show));
                }
            }
            return RedirectToAction(nameof(UserController.Show));
        }
        /// <summary>
        /// Метод смены пароля администратором, пользователю
        /// </summary>
        /// <param name="applicationUserViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ApplicationUserViewModel applicationUserViewModel)
        {
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    ApplicationUserDTO? userDTO = await _applicationUserService.GetUser(userId);
                    if (userDTO != null)
                    {
                        // Проверяем является ли он админом
                        if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "admin"))
                        {
                            // Меняем пароль
                            bool result = await _applicationUserService.ChangePassword(Guid.Parse(applicationUserViewModel.Id ?? ""), applicationUserViewModel.Password ?? "");
                            if (result)
                            {
                                TempData["Message"] = "Пароль успешно изменен";
                                return RedirectToAction(nameof(UserController.ShowCurrentUser), applicationUserViewModel);
                            }
                            TempData["Message"] = "Неудача, не удалось узменить пароль";
                            return RedirectToAction(nameof(UserController.ShowCurrentUser), applicationUserViewModel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, на окно вывода всех пользователей
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке изменить пароль произошла ошибка!";
                    return RedirectToAction(nameof(UserController.Show));
                }
            }
            return RedirectToAction(nameof(UserController.Show));
        }
    }
}
