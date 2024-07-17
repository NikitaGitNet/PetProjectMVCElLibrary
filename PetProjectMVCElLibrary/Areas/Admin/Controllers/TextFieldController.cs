using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.ApplicationUser;
using BLL.Models.DTO.Author;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Genre;
using BLL.Models.DTO.TextField;
using BLL.Services.ApplicationUser;
using BLL.Services.Author;
using BLL.Services.Book;
using BLL.Services.Genre;
using BLL.Services.TextField;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Areas.Admin.ViewModel.TextField;
using PetProjectMVCElLibrary.Controllers;
using PetProjectMVCElLibrary.Interfaces.Book;
using PetProjectMVCElLibrary.Service;
using PetProjectMVCElLibrary.ViewModel.Authorization;
using PetProjectMVCElLibrary.ViewModel.Book;

namespace PetProjectMVCElLibrary.Areas.Admin.Controllers
{
    /// <summary>
    /// Контроллер отвечающий за взаимодействие с TextField
    /// </summary>
    [Area("Admin")]
    public class TextFieldController : Controller
    {
        private readonly AppDbContext _context;
        private readonly TextFieldService textFieldService;
        private readonly IApplicationUserService applicationUserService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        public TextFieldController(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger logger)
        {
            _mapper = mapper;
            _context = context;
            textFieldService = new TextFieldService(context, mapper);
            _httpContextAccessor = httpContextAccessor;
            applicationUserService = new ApplicationUserService(context, mapper, signInManager, userManager);
            _logger = logger;
        }
        /// <summary>
        /// Метод принимает codeWord, если codeWord не defult, в представление попадает ViewModel этого TextField
        /// Если defult, то во ViewModel, поля будут пустые
        /// </summary>
        /// <param name="codeWord"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(string codeWord)
        {
            TextFieldDTO textFieldDTO = codeWord == default ? new TextFieldDTO() : (await textFieldService.GetTextFieldByCodeWord(codeWord) ?? new TextFieldDTO());
            return View(_mapper.Map<TextFieldViewModel>(textFieldDTO));
        }
        /// <summary>
        /// Метод принимает ViewModel TextField, сохраняет изменения
        /// </summary>
        /// <param name="textFieldViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(TextFieldViewModel textFieldViewModel)
        {
            if (ModelState.IsValid)
            {
                // Проверям авторизован ли пользователь
                Guid userId = Guid.Empty;
                if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
                {
                    try
                    {
                        // Получаем ДТО пользователя
                        ApplicationUserDTO? userDTO = await applicationUserService.GetUser(userId);
                        if (userDTO != null)
                        {
                            if (await applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "admin"))
                            {
                                // Мапим ViewModel в ДТО
                                TextFieldDTO textFieldDTO = _mapper.Map<TextFieldDTO>(textFieldViewModel);
                                // Сохраняем изменения
                                textFieldService.SaveTextField(textFieldDTO);
                                TempData["Message"] = "Текстовое поле успешно изменено";
                                return RedirectToAction(nameof(HomeController.Index));
                            }
                        }
                        // Если пользователь не найден, отправляем на экран авторизации
                        return RedirectToAction(nameof(AccountController.Login), new LoginViewModel());
                    }
                    catch (Exception ex)
                    {
                        // Генерим лог с сообщением об ошибке, редиректим на окно выбора книги
                        _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                        TempData["Message"] = "При попытке изменить текстовое поле произошла ошибка!";
                        return RedirectToAction(nameof(HomeController.Index));
                    }
                }
                // Если пользователь не авторизован, отправляем на экран авторизации
                return RedirectToAction(nameof(AccountController.Login), new LoginViewModel());
            }
            // Если валидация не пройдена, возвращаем на страницу редактирования
            return View(textFieldViewModel);
        }
    }
}
