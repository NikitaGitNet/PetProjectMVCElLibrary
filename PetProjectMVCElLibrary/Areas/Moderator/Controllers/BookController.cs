﻿using AutoMapper;
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
        private readonly IBookService _bookService;
        private readonly ICommentService _commentService;
        private readonly IAuthorService _authorService;
        private readonly IGenreService _genreService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnviroment;
        private readonly ILogger<FileLogger> _logger;
        public BookController(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment hostingEnviroment, UserManager<DAL.Domain.Entities.ApplicationUser> userManager, ILogger<FileLogger> logger)
        {
            _context = context;
            _mapper = mapper;
            _bookService = new BookService(context, mapper);
            _commentService = new CommentService(context, mapper);
            _authorService = new AuthorService(context, mapper);
            _genreService = new GenreService(context, mapper);
            _applicationUserService = new ApplicationUserService(context, mapper, signInManager, userManager);
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
            try
            {
                // Получаем ДТО книги, если код ИД пустой, возращаем пустое ДТО, иначе получаем ДТО по ИД
                BookDTO? bookDTO = id == default ? new BookDTO() : await _bookService.GetBook(id);
                return View(_mapper.Map<BookViewModel>(bookDTO));
            }
            catch (Exception ex)
            {
                // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                TempData["Message"] = "При попытке получить книгу произошла ошибка!";
                return RedirectToAction(nameof(HomeController.Index));
            }
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
                // Получаем ИД пользователя
                Guid userId = Guid.Empty;
                if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
                {
                    try
                    {
                        // Проверяем является ли пользователь модератором
                        if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userId.ToString()), "moderator"))
                        {
                            // Мапим ViewModel книги в ДТО
                            BookDTO bookDTO = _mapper.Map<BookDTO>(bookViewModel);
                            bookDTO.TitleImagePath = titleImageFile.FileName;
                            using (FileStream stream = new FileStream(Path.Combine(_hostingEnviroment.WebRootPath, "images/", titleImageFile.FileName), FileMode.Create))
                            {
                                titleImageFile.CopyTo(stream);
                            }
                            // Если есть автор, проверяем новый ли это автор
                            if (bookDTO.AuthorName != null)
                            {
                                // Если автор не новый связываем его с книгой
                                AuthorDTO? authorDTO = await _authorService.GetAuthorByName(bookDTO.AuthorName);
                                if (authorDTO != null)
                                {
                                    bookDTO.AuthorName = authorDTO.Name;
                                    bookDTO.AuthorId = authorDTO.Id;
                                }
                                else
                                {
                                    // Если новый, добавляем его в бд, связываем с книгой
                                    AuthorDTO newAuthorDTO = new() { Name = bookDTO.AuthorName, Id = Guid.NewGuid() };
                                    _authorService.CreateAuthor(newAuthorDTO);
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
                                GenreDTO? genreDTO = await _genreService.GetGenreByName(bookDTO.GenreName);
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
                                    _genreService.CreateGenre(newgenreDTO);
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
                            // Добавляем/обновляем книгу в БД
                            if (bookDTO.Id == default)
                            {
								bookDTO.DateAdded = DateTime.Now;
                                bookDTO.Id = Guid.NewGuid();
								_bookService.CreateBook(bookDTO);
								TempData["Message"] = "Книга успешно добавлена";
							}
                            else
                            {
								_bookService.UpdateBook(bookDTO);
								TempData["Message"] = "Книга успешно обновлена";
							}
                        }
                    }
                    catch (Exception ex)
                    {
                        // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                        _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                        TempData["Message"] = "При попытке редактировать/добавить книгу произошла ошибка!";
                    }
                }    
            }
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
            // Получаем ИД пользователя
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Проверяем является ли пользователь модератором
                    if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userId.ToString()), "moderator"))
                    {
                        // Получаем ДТО книги
                        BookDTO? bookDTO = await _bookService.GetBook(bookViewModel.Id);
                        if (bookDTO != null)
                        {
                            // Если ДТО не null, удаляем книгу
                            _bookService.DeleteBook(bookDTO.Id);
                            TempData["Message"] = "Книга успешно удалена";
                        }
                        else 
                        {
                            TempData["Message"] = "Не удалось удалить книгу, книга не найдена";
                        }
                    }
                }
                catch(Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель админа
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке удалить книгу произошла ошибка!";
                }
            }
            return RedirectToAction(nameof(BookController.BooksShow));
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
            // Получаем ИД пользователя
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
					// Проверяем является ли пользователь модератором
					if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userId.ToString()), "moderator"))
					{
						// Если называние не null
						if (model.Title != null)
						{
							// Получаем коллекцию ДТО книг
							IEnumerable<BookDTO> bookDTOs = await _bookService.GetAllBooks();
							// Ищем книгу с таким же названием
							bookDTOs = bookDTOs.Where(x => (x.Title ?? "").ToUpper().Contains(model.Title.ToUpper()));
							// Мапим во ViewModel, передаем в представление, возвращаем представление
							IEnumerable<BookViewModel> bookViewModels = _mapper.Map<IEnumerable<BookViewModel>>(bookDTOs);
							if (!bookViewModels.Any())
							{
								TempData["Message"] = "Книги не найдены";
							}
							TempData["Message"] = "Книги по вашему запросу:";
							return View("BooksShow", bookViewModels);
						}
						TempData["Message"] = "Введите название книги";
						return RedirectToAction(nameof(BookController.BooksShow));
					}
				}
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке найти книгу произошла ошибка!";
                    return RedirectToAction(nameof(BookController.BooksShow));
                }
            }
			TempData["Message"] = "При попытке найти книгу произошла ошибка!";
			return RedirectToAction(nameof(HomeController.Index));
        }
        /// <summary>
        /// Метод вывода всех имеющихся книг на экран через панель модератора
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> BooksShow()
        {
            IEnumerable<BookViewModel> booksViewModels = new List<BookViewModel>();
            // Получаем ИД пользователя
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Проверяем является ли пользователь модератором
                    if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userId.ToString()), "moderator"))
                    {
                        // Получаем ДТО книг, сортируем по названию, мапим во вью модел, передаем в представление
                        IEnumerable<BookDTO> bookDTOs = await _bookService.GetAllBooks();
                        bookDTOs = bookDTOs.OrderBy(x => x.Title);
                        booksViewModels = _mapper.Map<IEnumerable<BookViewModel>>(bookDTOs);
                    }
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, сообщаем пользователю, что произошла ошибка
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке получить книги произошла ошибка!";
                }
            }
            return View(booksViewModels);
        }
        /// <summary>
        /// Метод вывода всех имеющихся авторов на экран через панель модератора
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AuthorShow()
        {
            IEnumerable<AuthorViewModel> authorViewModels = new List<AuthorViewModel>();
            // Получаем ИД пользователя
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Проверяем является ли пользователь модератором
                    if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userId.ToString()), "moderator"))
                    {
                        // Получаем ДТО книг, сортируем по названию, мапим во вью модел, передаем в представление
                        IEnumerable<AuthorDTO> authorDTOs = await _authorService.GetAllAuthors();
                        authorDTOs = authorDTOs.OrderBy(x => x.Name);
                        authorViewModels = _mapper.Map<IEnumerable<AuthorViewModel>>(authorDTOs);
                    }
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, сообщаем пользователю, что произошла ошибка
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке получить авторов произошла ошибка!";
                }
            }
            // Возвращаем представление с коллекцией ViewModel
            return View(authorViewModels);
        }
        /// <summary>
        /// Метод для вывода всех имеющихся жанров на экран через панель модератора
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GenreShow()
        {
            IEnumerable<GenreViewModel> genreViewModels = new List<GenreViewModel>();
            // Получаем ИД пользователя
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Проверяем является ли пользователь модератором
                    if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userId.ToString()), "moderator"))
                    {
                        // Получаем ДТО жанров, сортируем по названию, мапим во вью модел, передаем в представление
                        IEnumerable<GenreDTO> genreDTOs = await _genreService.GetAllGenres();
                        genreDTOs = genreDTOs.OrderBy(x => x.Name);
                        genreViewModels = _mapper.Map<IEnumerable<GenreViewModel>>(genreDTOs);
                    }
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, сообщаем пользователю, что произошла ошибка
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке получить книги произошла ошибка!";
                }
            }
            // Возвращаем представление с коллекцией ViewModel
            return View(genreViewModels);
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
			try
			{
				// Если ИД дефолт, возращаем пустое ДТО, если не дефолт, достаем из базы, мапим в ДТО, возвращаем
				GenreDTO genreDTO = id == default ? new GenreDTO() : await _genreService.GetGenre(id) ?? new GenreDTO();
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
                // Получаем ИД пользователя
                Guid userId = Guid.Empty;
                if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
                {
                    try
                    {
						// Проверяем является ли пользователь модератором
						if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userId.ToString() ?? ""), "moderator"))
						{
                            GenreDTO? genreDTO = new GenreDTO();
                            if (model.Id != default)
                            {
                                genreDTO = await _genreService.GetGenre(model.Id);
                                if (genreDTO != null)
                                {
                                    genreDTO.Name = model.Name;
									if (_genreService.UpdateGenre(genreDTO))
									{
										TempData["Message"] = "Жанр успешно обновлен";
									}
								}
                            }
                            else
                            {
								// Делаем поиск жанра по имени, если находим, редиректим в панель админа, выводим сообщение
								genreDTO = await _genreService.GetGenreByName(model.Name ?? "");
								if (genreDTO != null)
								{
									TempData["Message"] = "Жанр с таким названием уже существует";
								}
                                else
                                {
									// Мапим ViewModel в ДТО, добавляем в базу
									genreDTO = _mapper.Map<GenreDTO>(model);
                                    genreDTO.Id = Guid.NewGuid();
									if (_genreService.CreateGenre(genreDTO))
									{
										TempData["Message"] = "Автор успешно добавлен";
									}
									else
									{
										TempData["Message"] = "При попытке добавить/обновить жанр произошла ошибка";
									}
								}
							}
						}
					}
                    catch (Exception ex)
                    {
                        // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                        _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                        TempData["Message"] = "При попытке обновления/добавления жанра произошла ошибка!";
                    }
                }
            }
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
            // Получаем ИД пользователя
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Проверяем является ли пользователь модератором
                    if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userId.ToString()), "moderator"))
                    {
                        // Получаем ДТО жанра
                        GenreDTO? genreDTO = await _genreService.GetGenre(model.Id);
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
                                _bookService.UpdateBooksRange(bookDTOs);
                            }
                            // Удаляем жанр
                            _genreService.DeleteGenre(genreDTO.Id);
                            TempData["Message"] = "Удаление жанра прошло успешно";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке удаления жанра произошла ошибка!";
                }
            }
            return RedirectToAction(nameof(BookController.GenreShow));
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
			try
			{
				// Если ИД дефолт, возращаем пустое ДТО, если не дефолт, достаем из базы, мапим в ДТО, возвращаем
				AuthorDTO authorDTO = id == default ? new AuthorDTO() : await _authorService.GetAuthor(id) ?? new AuthorDTO();
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
                // Получаем ИД пользователя
                Guid userId = Guid.Empty;
                if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
                {
                    try
                    {
                        // Проверяем является ли пользователь модератором
                        if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userId.ToString() ?? ""), "moderator"))
                        {
							AuthorDTO? authorDTO = new AuthorDTO();
                            // Если у ViewModel 
							if (model.Id != default)
                            {
                                authorDTO = await _authorService.GetAuthor(model.Id);
                                if (authorDTO != null)
                                {
									authorDTO.Name = model.Name;
                                    if (_authorService.UpdateAuthor(authorDTO))
                                    {
										TempData["Message"] = "Автор успешно обновлен";
                                    }
                                }
							}
                            else
                            {
								// Делаем поиск автора по имени, если находим, редиректим в панель админа, выводим сообщение
								authorDTO = await _authorService.GetAuthorByName(model.Name ?? "");
								if (authorDTO != null)
								{
									TempData["Message"] = "Автор с таким названием уже существует";
									return RedirectToAction(nameof(BookController.AddAuthor), model);
								}
                                else
                                {
									// Мапим ViewModel в ДТО, добавляем в базу
									authorDTO = _mapper.Map<AuthorDTO>(model);
                                    authorDTO.Id = Guid.NewGuid();
									if (_authorService.CreateAuthor(authorDTO))
									{
										TempData["Message"] = "Автор успешно добавлен";
										return RedirectToAction(nameof(HomeController.Index));
									}
                                    else
                                    {
										TempData["Message"] = "При попытке добавить/обновить автора произошла ошибка";
									}
								}
							}
						}
                    }
                    catch (Exception ex)
                    {
                        // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                        _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                        TempData["Message"] = "При попытке обновления/добавления автора произошла ошибка!";
                    }
                }
            }
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
            // Получаем ИД пользователя
            Guid userId = Guid.Empty;
            if (CheckUser.IsUserTry(_httpContextAccessor, out userId))
            {
                try
                {
                    // Проверяем является ли пользователь модератором
                    if (await _applicationUserService.IsUserRoleConfirm(Guid.Parse(userId.ToString()), "moderator"))
                    {
                        // Получаем ДТО автора
                        AuthorDTO? authorDTO = await _authorService.GetAuthor(model.Id);
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
                                _bookService.UpdateBooksRange(bookDTOs);
                            }
                            // Удаляем автора
                            _authorService.DeleteAuthor(model.Id);
                        }
                        TempData["Message"] = "Удаление автора прошло успешно";
                    }
                }
                catch(Exception ex) 
                {
                    // Генерим лог с сообщением об ошибке, редиректим на панель модератора
                    _logger.LogError(DateTime.Now + "\r\n" + ex.Message);
                    TempData["Message"] = "При попытке удалении автора произошла ошибка!";
                }
            }
            return RedirectToAction(nameof(BookController.AuthorShow));
        }
    }
}
