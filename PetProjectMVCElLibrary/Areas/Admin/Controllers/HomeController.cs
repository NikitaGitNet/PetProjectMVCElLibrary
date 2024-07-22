using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.ApplicationUser;
using BLL.Models.DTO.TextField;
using BLL.Services.ApplicationUser;
using BLL.Services.TextField;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Areas.Admin.ViewModel.TextField;
using PetProjectMVCElLibrary.Service;
using PetProjectMVCElLibrary.Service.Logger;

namespace PetProjectMVCElLibrary.Areas.Admin.Controllers
{
    /// <summary>
    /// Контроллер отвечающий за вывод данных/информации в панель администратора
    /// </summary>
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly TextFieldService _textFieldService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FileLogger> _logger;
        public HomeController(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<FileLogger> logger)
        {
            _signInManager = signInManager;
            _mapper = mapper;
            _context = context;
            _textFieldService = new TextFieldService(context, mapper);
            _httpContextAccessor = httpContextAccessor;
            _applicationUserService = new ApplicationUserService(context, mapper, signInManager, userManager);
            _logger = logger;
        }
        /// <summary>
        /// Метод возвращает представление панели администратора
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
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
                            return View();
                        }
                    }
                }
                catch (Exception ex) 
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель админа
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке загрузить панель администратора произошла ошибка!";
                    return RedirectToAction(nameof(HomeController.CommonIndex));
                }
            }
            return RedirectToAction(nameof(HomeController.CommonIndex));
        }
        /// <summary>
        /// Метод редиректит на стартовую страницу, если проблемы с авторизацией
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CommonIndex()
        {
            await _signInManager.SignOutAsync();
            // Получаем ДТО с помощью TextFieldService
            TextFieldViewModel textFieldViewModel = new TextFieldViewModel();
            try
            {
                TextFieldDTO? textFieldDTO = new TextFieldDTO();
                textFieldDTO = await _textFieldService.GetTextFieldByCodeWord("PageIndex");
                textFieldViewModel = _mapper.Map<TextFieldViewModel>(textFieldDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                TempData["Message"] = "При попытке загрузить панель администратора произошла ошибка!";
            }
            return View("~/Views/Home/Index.cshtml", textFieldViewModel);
        }
    }
}
