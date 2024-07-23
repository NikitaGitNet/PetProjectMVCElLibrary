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
using BLL.Services.Comment;
using BLL.Services.Genre;
using BLL.Services.TextField;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Areas.Admin.Interfaces.ViewModel;
using PetProjectMVCElLibrary.Areas.Admin.ViewModel.TextField;
using PetProjectMVCElLibrary.Controllers;
using PetProjectMVCElLibrary.Service;
using PetProjectMVCElLibrary.Service.Logger;
using PetProjectMVCElLibrary.ViewModel.Author;
using PetProjectMVCElLibrary.ViewModel.Authorization;
using PetProjectMVCElLibrary.ViewModel.Book;
using PetProjectMVCElLibrary.ViewModel.Genre;

namespace PetProjectMVCElLibrary.Areas.Moderator.Controllers
{
    [Area("Moderator")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBookService _bookService;
        private readonly ICommentService _commentService;
        private readonly IAuthorService _authorService;
        private readonly IGenreService _genreService;
        private readonly IApplicationUserService _applicationUserService;
		private readonly ITextFieldService _textFieldService;
		private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FileLogger> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public HomeController(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<FileLogger> logger)
        {
            _signInManager = signInManager;
            _context = context;
            _mapper = mapper;
            _bookService = new BookService(context, mapper);
            _commentService = new CommentService(context, mapper);
            _authorService = new AuthorService(context, mapper);
            _genreService = new GenreService(context, mapper);
			_textFieldService = new TextFieldService(context, mapper);
			_applicationUserService = new ApplicationUserService(context, mapper, signInManager, userManager);
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        /// <summary>
        /// Метод вывода информации в панель модератора
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Получаем ИД пользователя
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Проверяем является ли пользователь модератором
                    if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userId.ToString()), "moderator"))
                    {
                        // Возвращаем представление панели модератора
                        return View();
                    }
                }
                catch (Exception ex) 
                {
                    // Генерим лог с сообщением об ошибке
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                }
            }
			// Возвращаем представление стартовой страницы, передаем сообщение об ошибке
			TempData["Message"] = "При открыть панель модератора произошла ошибка!";
            return View("~/Views/Home/Index.cshtml", new TextFieldViewModel());
        }
    }
}
