using AutoMapper;
using BLL.Interfaces;
using BLL.Interfaces.DTO;
using BLL.Models.DTO.ApplicationUser;
using BLL.Models.DTO.Author;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Genre;
using BLL.Models.DTO.TextField;
using BLL.Services;
using BLL.Services.ApplicationUser;
using BLL.Services.Author;
using BLL.Services.Book;
using BLL.Services.Comment;
using BLL.Services.Genre;
using BLL.Services.TextField;
using DAL.Domain;
using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository.Book;
using DAL.Domain.Interfaces.Repository.Genre;
using DAL.Domain.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Areas.Admin.ViewModel.TextField;
using PetProjectMVCElLibrary.Controllers;
using PetProjectMVCElLibrary.Service;
using PetProjectMVCElLibrary.Service.Logger;
using PetProjectMVCElLibrary.ViewModel.Author;
using PetProjectMVCElLibrary.ViewModel.Book;
using PetProjectMVCElLibrary.ViewModel.Genre;

namespace PetProjectMVCElLibrary.Areas.Moderator.Controllers
{
    [Area("Moderator")]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBookService bookService;
        private readonly ICommentService commentService;
        private readonly IAuthorService authorService;
        private readonly IGenreService genreService;
        private readonly IApplicationUserService applicationUserService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnviroment;
        private readonly ILogger<FileLogger> _logger;
        public BookController(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment hostingEnviroment, UserManager<DAL.Domain.Entities.ApplicationUser> userManager, ILogger<FileLogger> logger)
        {
            _context = context;
            _mapper = mapper;
            bookService = new BookService(context, mapper);
            commentService = new CommentService(context, mapper);
            authorService = new AuthorService(context, mapper);
            genreService = new GenreService(context, mapper);
            applicationUserService = new ApplicationUserService(context, mapper, signInManager, userManager);
            _httpContextAccessor = httpContextAccessor;
            _hostingEnviroment = hostingEnviroment;
            _logger = logger;
        }
        /// <summary>
        /// Метод принимает codeWord, если codeWord не defult, в представление попадает ViewModel этой книги
        /// Если defult, то во ViewModel, поля будут пустые
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            // Проверям авторизован ли пользователь
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                BookDTO? bookDTO = new BookDTO();
                try
                {
                    // Получаем ДТО книги, если код ИД пустой, возращаем пустое ДТО, иначе получаем ДТО по ИД
                    bookDTO = id == default ? new BookDTO() : await bookService.GetBook(id);
                    if (bookDTO != null)
                    {
                        return View(_mapper.Map<BookViewModel>(bookDTO));
                    }
                    TempData["Message"] = "Не удалось найти книгу для редактирования";
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке получить книгу произошла ошибка!";
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            // Если пользователь не авторизован, отправляем на экран авторизации
            return RedirectToAction(nameof(HomeController.Index));
        }
        /// <summary>
        /// Метод редактирования, книги
        /// </summary>
        /// <param name="bookViewModel"></param>
        /// <param name="titleImageFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(BookViewModel bookViewModel, IFormFile titleImageFile)
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
                            // Проверяем является ли пользователь модератором
                            if (await applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "moderator"))
                            {
                                // Мапим ViewModel книги в ДТО
                                BookDTO bookDTO = _mapper.Map<BookDTO>(bookViewModel);
                                // Если картинка есть, сохраняем картинку
                                if (titleImageFile != null)
                                {
                                    bookDTO.TitleImagePath = titleImageFile.FileName;
                                    using (FileStream stream = new FileStream(Path.Combine(_hostingEnviroment.WebRootPath, "images/", titleImageFile.FileName), FileMode.Create))
                                    {
                                        titleImageFile.CopyTo(stream);
                                    }
                                }
                                // Если есть автор проверяем новый ли это автор
                                if (bookDTO.AuthorName != null)
                                {
                                    // Если автор не новый связываем его с книгой
                                    AuthorDTO? authorDTO = await authorService.GetAuthorByName(bookDTO.AuthorName);
                                    if (authorDTO != null)
                                    {
                                        bookDTO.AuthorName = authorDTO.Name;
                                        bookDTO.AuthorId = authorDTO.Id;
                                    }
                                    else
                                    {
                                        // Если новый, добавляем его в бд, связываем с книгой
                                        AuthorDTO newAuthorDTO = new() { Name = bookDTO.AuthorName, Id = Guid.NewGuid() };
                                        authorService.CreateAuthor(newAuthorDTO);
                                        bookDTO.AuthorName = newAuthorDTO.Name;
                                        bookDTO.AuthorId = newAuthorDTO.Id;
                                    }
                                }
                                else
                                {
                                    // Если автор не указан, связываем книгу с неизвестным автором
                                    bookDTO.AuthorName = UnknownAuthor.Name;
                                    bookDTO.AuthorId = new Guid(UnknownAuthor.Id);
                                }
                                // Если указан жанр, проверяем новый ли это жанр
                                if (bookDTO.GenreName != null)
                                {
                                    // Получаем ДТО жанра
                                    GenreDTO? genreDTO = await genreService.GetGenreByName(bookDTO.GenreName);
                                    // Если жанр найден, связываем его с книгой
                                    if (genreDTO != null)
                                    {
                                        bookDTO.GenreName = genreDTO.Name;
                                        bookDTO.GenreId = genreDTO.Id;
                                    }
                                    else
                                    {
                                        // Если не найден, добавляем в БД новый жанр, связываем его с книгой
                                        GenreDTO newgenreDTO = new() { Name = bookDTO.GenreName, Id = Guid.NewGuid() };
                                        genreService.CreateGenre(newgenreDTO);
                                        bookDTO.GenreName = newgenreDTO.Name;
                                        bookDTO.GenreId = newgenreDTO.Id;
                                    }
                                }
                                else
                                {
                                    // Если жанр не указан, связываем с неизвестным жанром
                                    bookDTO.GenreName = UnknownGenre.Name;
                                    bookDTO.GenreId = new Guid(UnknownGenre.Id);
                                }
                                // Указываем дату добавления книги, добавляем/обновляем книгу в БД
                                bookDTO.DateAdded = DateTime.Now;
                                bookService.CreateBook(bookDTO);
                                TempData["Message"] = "Книга успешно добавлена/обновлена";
                                // Редиректим в панель модератора
                                return RedirectToAction(nameof(HomeController.Index));
                            }
                        }
                        TempData["Message"] = "При попытке редактировать/добавить книгу произошла ошибка!";
                    }
                    catch (Exception ex)
                    {
                        // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                        _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                        TempData["Message"] = "При попытке редактировать/добавить книгу произошла ошибка!";
                        return RedirectToAction(nameof(HomeController.Index));
                    }
                }    
            }
            TempData["Message"] = "Поля формы заполнены не корректно";
            return View(bookViewModel);
        }
        /// <summary>
        /// Метод удаления книги
        /// </summary>
        /// <param name="bookViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteBook(BookViewModel bookViewModel)
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
                        // Проверяем является ли пользователь модератором
                        if (await applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "moderator"))
                        {
                            // Получаем ДТО книги
                            BookDTO? bookDTO = await bookService.GetBook(bookViewModel.Id);
                            if (bookDTO != null)
                            {
                                // Если ДТО не null, удаляем книгу
                                bookService.DeleteBook(bookDTO.Id);
                            }
                            return RedirectToAction(nameof(HomeController.Index));
                        }
                    }
                }
                catch(Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель админа
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке удалить книгу произошла ошибка!";
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            TempData["Message"] = "При попытке удалить книгу произошла ошибка!";
            return RedirectToAction(nameof(HomeController.Index));
        }
        /// <summary>
        /// Метод поиска книги по названию
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SearchByName(BookViewModel model)
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
                        // Проверяем является ли пользователь модератором
                        if (await applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "moderator"))
                        {
                            // Если называние не null
                            if (model.Title != null)
                            {
                                // Получаем коллекцию ДТО книг
                                IEnumerable<BookDTO> bookDTOs = await bookService.GetAllBooks();
                                // Ищем книгу с таким же названием
                                bookDTOs = bookDTOs.Where(x => (x.Title ?? "").ToUpper().Contains(model.Title.ToUpper()));
                                // Мапим во ViewModel, передаем в представление, возвращаем представление
                                IEnumerable<BookViewModel> bookViewModels = _mapper.Map<IEnumerable<BookViewModel>>(bookDTOs);
                                return View("BooksShow", bookViewModels);
                            }
                            TempData["Message"] = "При попытке найти книгу произошла ошибка!";
                            return RedirectToAction(nameof(BookController.BooksShow));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке удалить книгу произошла ошибка!";
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            return RedirectToAction(nameof(HomeController.Index));
        }
        /// <summary>
        /// Метод вывода книг на экран
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> BooksShow()
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
                        // Проверяем является ли пользователь модератором
                        if (await applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "moderator"))
                        {
                            // Получаем ДТО книг, сортируем по названию, мапим во вью модел, передаем в представление
                            IEnumerable<BookDTO> bookDTOs = await bookService.GetAllBooks();
                            bookDTOs = bookDTOs.OrderBy(x => x.Title);
                            IEnumerable<BookViewModel> booksViewModels = _mapper.Map<IEnumerable<BookViewModel>>(bookDTOs);
                            return View(booksViewModels);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке получить книгу произошла ошибка!";
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            TempData["Message"] = "При попытке получить книгу произошла ошибка!";
            return RedirectToAction(nameof(HomeController.Index));
        }
        /// <summary>
        /// Метод получения и вывода жанра по ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddGenre(Guid id)
        {
            // Проверям авторизован ли пользователь
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Если ИД дефолт, возращаем пустое ДТО, если не дефолт, достаем из базы, мапим в ДТО, возвращаем
                    GenreDTO genreDTO = id == default ? new GenreDTO() : await genreService.GetGenre(id) ?? new GenreDTO();
                    return View(_mapper.Map<GenreViewModel>(genreDTO));
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке получить жанр произошла ошибка!";
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }    
            // Если пользователь не авторизован, отправляем на экран авторизации
            return RedirectToAction(nameof(HomeController.Index));
        }
        /// <summary>
        /// Метод добавления/редактирования жанра
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddGenre(GenreViewModel model)
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
                            // Проверяем является ли пользователь модератором
                            if (await applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "moderator"))
                            {
                                GenreDTO? genreDTO = _mapper.Map<GenreDTO>(model);
                                // Делаем поиск жанра по имени, если находим, редиректим в панель админа, выводим сообщение
                                GenreDTO? newGenreDTO = await genreService.GetGenreByName(model.Name ?? "");
                                if (newGenreDTO != null)
                                {
                                    TempData["Message"] = "Жанр с таким названием уже существует";
                                    return RedirectToAction(nameof(BookController.AddGenre), model.Id);
                                }
                                else
                                {
                                    // Если не находим, создаем новый жанр
                                    if (genreDTO != null)
                                    {
                                        genreService.CreateGenre(genreDTO);
                                        TempData["Message"] = "Жанр успешно добавлен/обновлен";
                                    }
                                }
                                return RedirectToAction(nameof(BookController.AddGenre), model.Id);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                        _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                        TempData["Message"] = "При попытке обновления/добавления жанра произошла ошибка!";
                        return RedirectToAction(nameof(HomeController.Index));
                    }
                }
            }
            TempData["Message"] = "Поле заполнено не корректно";
            return View(model);
        }
        /// <summary>
        /// Метод удаления жанра
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteGenre(GenreViewModel model)
        {
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Получаем ДТО пользователя
                    ApplicationUserDTO? userDTO = await applicationUserService.GetUser(userId);
                    if (userDTO != null)
                    {
                        // Проверяем является ли пользователь модератором
                        if (await applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "moderator"))
                        {
                            // Получаем ДТО жанра
                            GenreDTO? genreDTO = await genreService.GetGenre(model.Id);
                            if (genreDTO != null)
                            {
                                // Если жанр связан с книгами, присваиваем книгам неизвестный жанр, обновляем книги
                                if (genreDTO.Books != null && genreDTO.Books.Any())
                                {
                                    IEnumerable<BookDTO> bookDTOs = genreDTO.Books;
                                    foreach (BookDTO bookDTO in bookDTOs)
                                    {
                                        bookDTO.GenreId = Guid.Parse(UnknownGenre.Id);
                                        bookDTO.GenreName = UnknownGenre.Name;
                                    }
                                    bookService.UpdateBooksRange(bookDTOs);
                                }
                                // Удаляем жанр
                                genreService.DeleteGenre(genreDTO.Id);
                                TempData["Message"] = "Удаление жанра прошло успешно";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке удаления жанра произошла ошибка!";
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            return RedirectToAction(nameof(HomeController.Index));
        }
        /// <summary>
        /// Метод получения и вывода автора по ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddAuthor(Guid id)
        {
            // Проверям авторизован ли пользователь
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Если ИД дефолт, возращаем пустое ДТО, если не дефолт, достаем из базы, мапим в ДТО, возвращаем
                    AuthorDTO authorDTO = id == default ? new AuthorDTO() : await authorService.GetAuthor(id) ?? new AuthorDTO();
                    return View(_mapper.Map<AuthorViewModel>(authorDTO));
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке получить автора произошла ошибка!";
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            // Если пользователь не авторизован, отправляем на экран авторизации
            return RedirectToAction(nameof(HomeController.Index));
        }
        /// <summary>
        /// Метод добавления/редактирования автора
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddAuthor(AuthorViewModel model)
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
                            // Проверяем является ли пользователь модератором
                            if (await applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "moderator"))
                            {
                                AuthorDTO? authorDTO = _mapper.Map<AuthorDTO>(model);
                                // Делаем поиск жанра по имени, если находим, редиректим в панель админа, выводим сообщение
                                AuthorDTO? newAuthorDTO = await authorService.GetAuthorByName(model.Name ?? "");
                                if (newAuthorDTO != null)
                                {
                                    TempData["Message"] = "Автор с таким названием уже существует";
                                    return RedirectToAction(nameof(BookController.AddAuthor), model.Id);
                                }
                                else
                                {
                                    // Если не находим, создаем нового автора
                                    if (authorDTO != null)
                                    {
                                        authorService.CreateAuthor(authorDTO);
                                        TempData["Message"] = "Автор успешно добавлен/обновлен";
                                    }
                                }
                                return RedirectToAction(nameof(BookController.AddGenre), model.Id);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                        _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                        TempData["Message"] = "При попытке обновления/добавления автора произошла ошибка!";
                        return RedirectToAction(nameof(HomeController.Index));
                    }
                }
            }
            TempData["Message"] = "Поле заполнено не корректно";
            return View(model);
        }
        /// <summary>
        /// Метод удаления автора
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteAuthor(AuthorViewModel model)
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
                        // Проверяем является ли пользователь модератором
                        if (await applicationUserService.IsUserRoleConfirm(Guid.Parse(userDTO.Id ?? ""), "moderator"))
                        {
                            // Получаем ДТО автора
                            AuthorDTO? authorDTO = await authorService.GetAuthor(model.Id);
                            if (authorDTO != null)
                            {
                                // Если не null, смотрим есть связанные с ним книги
                                if (authorDTO.Books != null && authorDTO.Books.Any())
                                {
                                    // Если есть связываем их с неизвестным автором
                                    IEnumerable<BookDTO> bookDTOs = authorDTO.Books;
                                    foreach (BookDTO bookDTO in bookDTOs)
                                    {
                                        bookDTO.AuthorId = Guid.Parse(UnknownAuthor.Id);
                                        bookDTO.AuthorName = UnknownAuthor.Name;
                                    }
                                    // Обновляем книги
                                    bookService.UpdateBooksRange(bookDTOs);
                                }
                                // Удаляем автора
                                authorService.DeleteAuthor(model.Id);
                            }
                            TempData["Message"] = "Удаление автора прошло успешно";
                        }
                    }
                }
                catch(Exception ex) 
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке удалении автора произошла ошибка!";
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            return RedirectToAction(nameof(HomeController.Index));
        }
    }
}
