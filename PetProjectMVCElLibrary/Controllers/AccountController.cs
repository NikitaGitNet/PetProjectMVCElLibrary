using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.ApplicationUser;
using BLL.Services.ApplicationUser;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Service.Logger;
using PetProjectMVCElLibrary.ViewModel.Authorization;

namespace PetProjectMVCElLibrary.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<FileLogger> _logger;
        private readonly IApplicationUserService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(AppDbContext context, SignInManager<ApplicationUser> signInManager, IMapper mapper, UserManager<ApplicationUser> userManager, ILogger<FileLogger> logger)
        {
            _applicationUserService = new ApplicationUserService(context, mapper, signInManager, userManager);
            _signInManager = signInManager;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        /// <summary>
        /// Возвращаем представление с формой авторизации
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _applicationUserService.SignInResultSucceeded(model.Email ?? "", model.Password ?? "", model.RememberMe))
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(nameof(LoginViewModel.Email), "Неверный email или пароль");
            }
            return View(model);
        }
        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Метод регистрации, возвращает представление для регистрации
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        /// <summary>
        /// Метод выполняющий логику авторизации
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Проверяем форму на валидацию
            if (ModelState.IsValid)
            {
                try
                {
                    // Дополнительно проверяем что юзернейм не null и не пустой
                    if (model.UserName != null && model.UserName != string.Empty)
                    {
                        // Получаем ДТО всех пользователей
                        IEnumerable<ApplicationUserDTO> users = await _applicationUserService.GetAllUsers();
                        // Проверяем, есть ли пользователь с таким же юзернеймом как во ViewModel
                        ApplicationUserDTO? applicationUserDTO = users.FirstOrDefault(x => x.UserName?.ToUpper() == model.UserName.ToUpper());
                        if (applicationUserDTO != null)
                        {
                            // Если есть возвращаем представление, выдаем ошибку
                            ViewData["Message"] = "Пользователь с таким именем уже существует";
                            return View(model);
                        }
                        if (model.UserName.Length > 20)
                        {
                            // Если юзернейм больше 20 символов, возвращаем представление, выдаем ошибку
                            ViewData["Message"] = "Длина имени больше 20 символов";
                            return View(model);
                        }
                        // Проверяем, есть ли пользователь с таким же email как во ViewModel
                        applicationUserDTO = users.FirstOrDefault(x => x.Email?.ToUpper() == model.Email?.ToUpper());
                        if (applicationUserDTO != null)
                        {
                            ViewData["Message"] = "Данная почта уже зарегестрированна";
                            return View(model);
                        }
                        // Создаем ДТО пользователя на основании ViewModel
                        ApplicationUserDTO newUser = _mapper.Map<ApplicationUserDTO>(model);
                        newUser.Id = Guid.NewGuid().ToString();
                        // Сохраняем в базу
                        bool result = await _applicationUserService.CreateUser(newUser);
                        if (result)
                        {
                            // Если все ОК, авторизуем, редиректим на главную
                            await _applicationUserService.SignIn(newUser);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке пройти регистрацию произошла ошибка!";
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            // Если не ОК, выдаем ошибку, возвращаем представление
            ViewData["Message"] = "При попытке пройти регистрацию произошла ошибка!";
            return View(model);
        }
    }
}
