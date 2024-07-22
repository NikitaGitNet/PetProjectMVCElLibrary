using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.ApplicationUser;
using BLL.Models.DTO.Author;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Comment;
using BLL.Models.DTO.Genre;
using BLL.Services.ApplicationUser;
using BLL.Services.Author;
using BLL.Services.Book;
using BLL.Services.Comment;
using BLL.Services.Genre;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Interfaces.Book;
using PetProjectMVCElLibrary.Service;
using PetProjectMVCElLibrary.Service.Logger;
using PetProjectMVCElLibrary.ViewModel.Author;
using PetProjectMVCElLibrary.ViewModel.Authorization;
using PetProjectMVCElLibrary.ViewModel.Book;
using PetProjectMVCElLibrary.ViewModel.Comment;

namespace PetProjectMVCElLibrary.Controllers
{
    /// <summary>
    /// Контроллер содержащий логику взаимодействия с книгами и комментариями к ним
    /// </summary>
    public class BookController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBookService _bookService;
        private readonly ICommentService _commentService;
        private readonly IAuthorService _authorService;
        private readonly IGenreService _genreService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FileLogger> _logger;
        public BookController(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<FileLogger> logger)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _bookService = new BookService(context, mapper);
            _commentService = new CommentService(context, mapper);
            _authorService = new AuthorService(context, mapper);
            _genreService = new GenreService(context, mapper);
            _applicationUserService = new ApplicationUserService(context, mapper, signInManager, userManager);
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// Метод вывода всех имеющихся книг на экран
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<BookViewModel> bookViewModels = new List<BookViewModel>();
            IEnumerable<BookDTO> bookDTOs = new List<BookDTO>();
            try
            {
                // Получаем ДТО книг
                bookDTOs = await _bookService.GetAllBooks();
                // Мапим ДТО во ViewModel
                bookViewModels = _mapper.Map<IEnumerable<BookViewModel>>(bookDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                // Сообщаем об ошибке, редиректим на стартовую страницу
                TempData["Message"] = "При попытке загрузить список книг произошла ошибка!";
                return RedirectToAction(nameof(HomeController.Index));
            }
            // Передаем ViewModel в представление, возращаем представление
            return View(bookViewModels);
        }
        /// <summary>
        /// Вывести конкретную книгу и комментарии к ней
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ShowCurrentBook(BookViewModel book)
        {
            IBookViewModel bookViewModel = new BookViewModel();
            try
            {
                // Получаем ДТО книг
                BookDTO? bookDTO = await _bookService.GetBook(book.Id);
                // Мапим во ViewModel
                bookViewModel = _mapper.Map<BookViewModel>(bookDTO);
                // Получаем ДТО комментариев
                IEnumerable<CommentDTO> commentDTOs = await _commentService.GetCommentsByBookId(book.Id);
                // Мапим комментарии во ViewModel
                bookViewModel.Comments = _mapper.Map<IEnumerable<CommentViewModel>>(commentDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                // Сообщаем об ошибке, редиректим на стартовую страницу
                TempData["Message"] = "При попытке загрузить книгe произошла ошибка!";
                return RedirectToAction(nameof(BookController.Index));
            }
            // Передаем ViewModel в представление, возвращаем представление
            return View(bookViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> SearchByName(string name)
        {
            try
            {
                // Если называние не null
                if (name != null)
                {
                    // Получаем коллекцию ДТО книг
                    IEnumerable<BookDTO> bookDTOs = await _bookService.GetAllBooks();
                    // Ищем книгу с таким же названием
                    bookDTOs = bookDTOs.Where(x => (x.Title ?? "").ToUpper().Contains(name.ToUpper()));
                    // Мапим во ViewModel, передаем в представление, возвращаем представление
                    IEnumerable<BookViewModel> bookViewModels = _mapper.Map<IEnumerable<BookViewModel>>(bookDTOs);
                    if (!bookViewModels.Any())
                    {
                        TempData["Message"] = "Книги не найдены";
                    }
                    return View("Index", bookViewModels);
                }
                TempData["Message"] = "Для поиска по названию необходимо заполнить поле ввода";
            }
            catch (Exception ex)
            {
                // Генерим лог с сообщением об ошибке, редиректим в HomeController.Index
                _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                TempData["Message"] = "При попытке удалить книгу произошла ошибка!";
            }
            return RedirectToAction(nameof(HomeController.Index));
        }
        [HttpGet]
        public async Task<IActionResult> SearchBookByAutor()
        { 
            try
            {
                IEnumerable<AuthorDTO> authorDTOs = await _authorService.GetAllAuthors();
                if (authorDTOs.Any())
                {
                    authorDTOs = authorDTOs.OrderBy(x => x.Name);
                }
                else
                {
                    TempData["Message"] = "Авторы не найдены";
                }
                return View(_mapper.Map<IEnumerable<AuthorViewModel>>(authorDTOs));
            }
            catch (Exception ex)
            {
                // Генерим лог с сообщением об ошибке, редиректим в HomeController.Index
                _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                TempData["Message"] = "При попытке вывести авторов произошла ошибка!";
            }
            return RedirectToAction(nameof(BookController.Index));
        }
        /// <summary>
        /// Поиск книг по автору
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SearchBookByAutor(Guid authorId)
        {
            IEnumerable<BookViewModel> bookViewModels = new List<BookViewModel>();
            try
            {
                // Получаем ДТО автора
                AuthorDTO? authorDTO = await _authorService.GetAuthor(authorId);
                // Если автор найден, выполняем логику
                if (authorDTO != null)
                {
                    // Получаем ДТО книг
                    IEnumerable<BookDTO> bookDTOs = await _bookService.GetBookByAuthor(authorId);
                    // Мапим во ViewModel
                    bookViewModels = _mapper.Map<IEnumerable<BookDTO>, IEnumerable<BookViewModel>>(bookDTOs);
                    // Выводи сообщение
                    TempData["Message"] = $"Список книг по запросу: автор - {authorDTO.Name}";
                }
                // Выводим сообщение, что автор не найден
                TempData["Message"] = $"Автор не найден";
            }
            catch (Exception ex)
            {
                // Сообщаем об ошибке, редиректим на стартовую страницу
                _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                TempData["Message"] = "При попытке загрузить список книг произошла ошибка!";
                return RedirectToAction(nameof(BookController.Index));
            }
            
            // Передаем ViewModel в представление, возращаем представление
            return View("Index", bookViewModels);
        }
        /// <summary>
        /// Поиск книг по жанру
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchBookByGenre(Guid genreId)
        {
            IEnumerable<BookViewModel> bookViewModels = new List<BookViewModel>();
            try
            {
                // Получаем ДТО жанра
                GenreDTO? genreDTO = await _genreService.GetGenre(genreId);
                // Если жанр найден, выполняем логику
                if (genreDTO != null)
                {
                    // Получаем ДТО книг
                    IEnumerable<BookDTO> bookDTOs = await _bookService.GetBookByGenre(genreId);
                    // Мапим во ViewModel
                    bookViewModels = _mapper.Map<IEnumerable<BookDTO>, IEnumerable<BookViewModel>>(bookDTOs);
                    // Выводи сообщение
                    TempData["Message"] = $"Список книг по запросу: жанр - {genreDTO.Name}";
                }
            }
            catch (Exception ex)
            {
                // Сообщаем об ошибке, редиректим на стартовую страницу
                _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                TempData["Message"] = "При попытке загрузить список книг произошла ошибка!";
                return RedirectToAction(nameof(BookController.Index));
            }
            return View("Index", bookViewModels);
        }
        /// <summary>
        /// Добавление кооментария к книге
        /// </summary>
        /// <param name="commentViewModel"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(CommentViewModel commentViewModel)
        {
            // Проверям авторизован ли пользователь
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                // Если авторизован, выполняем логику
                try
                {
                    // Получаем ДТО пользователя
                    ApplicationUserDTO? userDTO = await _applicationUserService.GetUser(userId);
                    // Если пользователь найден, получаем ДТО книги
                    if (userDTO != null)
                    {
                        // Получаем ДТО книги, если книга найдена
                        BookDTO? bookDTO = await _bookService.GetBook(commentViewModel.Id);
                        if (bookDTO != null)
                        {
                            // Генерим ДТО коммента
                            CommentDTO commentDTO = new CommentDTO()
                            {
                                CreateOn = DateTime.Now,
                                CommentText = commentViewModel.CommentText,
                                UserId = userId.ToString(),
                                BookId = commentViewModel.Id,
                                Email = userDTO.Email,
                                UserName = userDTO.UserName,
                            };
                            // Добавляем комментарий
                            _commentService.CreateComment(commentDTO);
                        }
                        // Редиректим на страницу книги
                        return RedirectToAction(nameof(BookController.ShowCurrentBook), _mapper.Map<BookViewModel>(bookDTO));
                    }
                    // Если пользователь не найден, отправляем на экран авторизации
                    return RedirectToAction(nameof(AccountController.Login), new LoginViewModel());
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на окно выбора книги
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке добавить комментарий произошла ошибка!";
                    return RedirectToAction(nameof(BookController.Index));
                }
            }
            // Если пользователь не авторизован, отправляем на экран авторизации
            return RedirectToAction(nameof(AccountController.Login), new LoginViewModel());
        }
        /// <summary>
        /// Метод позволяющий пользователю удалить собственный комментарий
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteComment(CommentViewModel model)
        {
            // Проверям авторизован ли пользователь
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                // Если авторизован, выполняем логику
                try
                {
                    // Получаем ДТО книги
                    BookDTO? bookDTO = await _bookService.GetBook(model.BookId);
                    // Если книга найдена
                    if (bookDTO != null)
                    {
                        // Удаляем комментарий, редиректим на страницу книги
                        _commentService.DeleteComment(model.Id);
                        return RedirectToAction(nameof(BookController.ShowCurrentBook), _mapper.Map<BookViewModel>(bookDTO));
                    }
                    // Если не найдена, редиректим на страницу вывода книг, пишем сообщение, что не удалось удалить комментарий
                    TempData["Message"] = "При попытке удалить комментарий произошла ошибка! Книга не найдена!";
                    return RedirectToAction(nameof(BookController.Index));
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на окно выбора книги
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке удалить комментарий произошла ошибка!";
                    return RedirectToAction(nameof(BookController.Index));
                }
            }
            // Если пользователь не авторизован, отправляем на экран авторизации
            return RedirectToAction(nameof(AccountController.Login), new LoginViewModel());
        }
    }
}
