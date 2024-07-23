using AutoMapper;
using BLL.Models.DTO.TextField;
using BLL.Services.TextField;
using DAL.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetProjectMVCElLibrary.Areas.Admin.ViewModel.TextField;
using PetProjectMVCElLibrary.Service.Logger;

namespace PetProjectMVCElLibrary.Controllers
{
    /// <summary>
    /// Стартовый контроллер, содержит методы для вывода текстовых полей.
    /// Поля заполняются администратором в панели администратора, за их редактирование отвечает TextFieldController
    /// </summary>
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly TextFieldService textFieldService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FileLogger> _logger;
        public HomeController(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger<FileLogger> logger)
        {
            _context = context;
            textFieldService = new TextFieldService(context, mapper);
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        /// <summary>
        /// Метод выводит стартовую страницу. Она же вкладка - "Главное" из хедера
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            TextFieldViewModel textFieldViewModel = new TextFieldViewModel();
            try
            {
                // Получаем ДТО TextField
                TextFieldDTO? textFieldDTO = await textFieldService.GetTextFieldByCodeWord("PageIndex");
                // Если ДТО не null
                if (textFieldDTO != null)
                {
                    // Маппим ее во ViewModel, передаем в предстовление
                    textFieldViewModel = _mapper.Map<TextFieldViewModel>(textFieldDTO);
                    return View(textFieldViewModel ?? new TextFieldViewModel { Text = "" });
                }
                // Если null, генерим лог, что произошла ошибка
                _logger.LogError($"{DateTime.Now}\r\nОшибка: TextField с ключевым словом PageIndex не найдена!");
            }
            catch (Exception ex)
            {
                // Генерим лог, выдаем пользователю сообщение, что произошла ошибка, передаем пустую ViewModel в представление
                _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                TempData["Message"] = "При попытке загрузить страницу произошла ошибка!";
                return View(new TextFieldViewModel());
            }
            // Возвращаем представление с пустым ViewModel
            return View(new TextFieldViewModel());
        }
        /// <summary>
        /// Метод выводит страницу с контактными данными. Она же вкладка - "Контакты" из хедера
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Contacts()
        {
            TextFieldViewModel textFieldViewModel = new TextFieldViewModel();
            try
            {
                // Получаем ДТО с помощью TextFieldService
                TextFieldDTO? textFieldDTO = await textFieldService.GetTextFieldByCodeWord("PageContacts");
                // Если ДТО не null
                if (textFieldDTO != null)
                {
                    // Маппим ее во ViewModel, передаем в предстовление
                    textFieldViewModel = _mapper.Map<TextFieldViewModel>(textFieldDTO);
                    return View(textFieldViewModel ?? new TextFieldViewModel { Text = "" });
                }
                // Если null, генерим лог, что произошла ошибка
                _logger.LogError($"{DateTime.Now}\r\nОшибка: TextField с ключевым словом PageContacts не найдена!");
            }
            catch (Exception ex)
            {
                _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                TempData["Message"] = "При попытке загрузить страницу произошла ошибка!";
                return View(new TextFieldViewModel());
            }
            // Возвращаем представление с пустым ViewModel
            return View(new TextFieldViewModel());
        }
    }
}
