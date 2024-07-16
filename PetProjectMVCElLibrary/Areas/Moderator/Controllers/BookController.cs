using AutoMapper;
using BLL.Interfaces;
using BLL.Interfaces.DTO;
using BLL.Models.DTO.Author;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Genre;
using BLL.Services;
using BLL.Services.ApplicationUser;
using BLL.Services.Author;
using BLL.Services.Book;
using BLL.Services.Comment;
using BLL.Services.Genre;
using DAL.Domain;
using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository.Book;
using DAL.Domain.Interfaces.Repository.Genre;
using DAL.Domain.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Controllers;
using PetProjectMVCElLibrary.Service;
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
        public BookController(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment hostingEnviroment, UserManager<DAL.Domain.Entities.ApplicationUser> userManager)
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
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            BookDTO bookDTO = id == default ? new BookDTO() : await bookService.GetBook(id);
            return View(_mapper.Map<BookViewModel>(bookDTO));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(BookViewModel bookViewModel, IFormFile titleImageFile)
        {
            if (ModelState.IsValid)
            {
                BookDTO bookDTO = _mapper.Map<BookDTO>(bookViewModel);
                if (titleImageFile != null)
                {
                    bookDTO.TitleImagePath = titleImageFile.FileName;
                    using (FileStream stream = new FileStream(Path.Combine(_hostingEnviroment.WebRootPath, "images/", titleImageFile.FileName), FileMode.Create))
                    {
                        titleImageFile.CopyTo(stream);
                    }
                }
                if (bookDTO.AuthorName != null)
                {
                    AuthorDTO? authorDTO = await authorService.GetAuthorByName(bookDTO.AuthorName);
                    if (authorDTO != null)
                    {
                        bookDTO.AuthorName = authorDTO.Name;
                        bookDTO.AuthorId = authorDTO.Id;
                    }
                    else
                    {
                        AuthorDTO newAuthorDTO = new() { Name = bookDTO.AuthorName, Id = Guid.NewGuid() };
                        authorService.CreateAuthor(newAuthorDTO);
                        bookDTO.AuthorName = newAuthorDTO.Name;
                        bookDTO.AuthorId = newAuthorDTO.Id;
                    }
                }
                else
                {
                    bookDTO.AuthorName = UnknownAuthor.Name;
                    bookDTO.AuthorId = new Guid(UnknownAuthor.Id);
                }
                if (bookDTO.GenreName != null)
                {
                    GenreDTO? genreDTO = await genreService.GetGenreByName(bookDTO.GenreName);
                    if (genreDTO != null)
                    {
                        bookDTO.GenreName = genreDTO.Name;
                        bookDTO.GenreId = genreDTO.Id;
                    }
                    else
                    {
                        GenreDTO newgenreDTO = new() { Name = bookDTO.GenreName, Id = Guid.NewGuid() };
                        genreService.CreateGenre(newgenreDTO);
                        bookDTO.GenreName = newgenreDTO.Name;
                        bookDTO.GenreId = newgenreDTO.Id;
                    }
                }
                else
                {
                    bookDTO.GenreName = UnknownGenre.Name;
                    bookDTO.GenreId = new Guid(UnknownGenre.Id);
                }
                bookDTO.DateAdded = DateTime.Now;
                bookService.CreateBook(bookDTO);
                return RedirectToAction(nameof(HomeController.Index));
            }
            return View(bookViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteBook(BookViewModel bookViewModel)
        {
            BookDTO bookDTO = await bookService.GetBook(bookViewModel.Id);
            if (bookDTO != null)
            {
                bookService.DeleteBook(bookDTO.Id);
            }
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
        [HttpPost]
        public async Task<IActionResult> SearchByName(BookViewModel model)
        {
            if (model.Title != null)
            {
                IEnumerable<BookDTO> bookDTOs = await bookService.GetAllBooks();
                bookDTOs = bookDTOs.Where(x => (x.Title ?? "").ToUpper() == model.Title.ToUpper());
                IEnumerable<BookViewModel> bookViewModels = _mapper.Map<IEnumerable<BookViewModel>>(bookDTOs);
                return View("BooksShow", bookViewModels);
            }
            return RedirectToAction(nameof(BooksShow));
        }
        [HttpGet]
        public async Task<IActionResult> BooksShow()
        {
            IEnumerable<BookDTO> bookDTOs = await bookService.GetAllBooks();
            bookDTOs = bookDTOs.OrderBy(x => x.Title);
            IEnumerable<BookViewModel> booksViewModels = _mapper.Map<IEnumerable<BookViewModel>>(bookDTOs);
            return View(booksViewModels);
        }
        public async Task<IActionResult> AddGenre(Guid id)
        {
            GenreDTO genreDTO = id == default ? new GenreDTO() : await genreService.GetGenre(id) ?? new GenreDTO();
            return View(_mapper.Map<GenreViewModel>(genreDTO));
        }
        [HttpPost]
        public async Task<IActionResult> AddGenre(GenreViewModel model)
        {
            if (ModelState.IsValid)
            {
                GenreDTO? genreDTO = _mapper.Map<GenreDTO>(model);
                GenreDTO? newGenreDTO = await genreService.GetGenreByName(model.Name ?? "");
                if (newGenreDTO != null)
                {
                    return View("~/Views/ErrorPage.cshtml", "Жанр с таким названием уже существует");
                }
                else
                {
                    if (genreDTO != null)
                    {
                        genreService.CreateGenre(genreDTO);
                    }
                }
                return RedirectToAction(nameof(HomeController.Index));
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteGenre(GenreViewModel model)
        { 
            GenreDTO? genreDTO = await genreService.GetGenre(model.Id);
            if (genreDTO != null)
            {
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
                genreService.DeleteGenre(genreDTO.Id);
            }
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
        public async Task<IActionResult> AddAuthor(Guid id)
        {
            AuthorDTO authorDTO = id == default ? new AuthorDTO() : await authorService.GetAuthor(id) ?? new AuthorDTO();
            return View(_mapper.Map<AuthorViewModel>(authorDTO));
        }
        [HttpPost]
        public async Task<IActionResult> AddAuthor(AuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                AuthorDTO? authorDTO = _mapper.Map<AuthorDTO>(model);
                AuthorDTO? newAuthorDTO = await authorService.GetAuthorByName(model.Name ?? "");
                if (newAuthorDTO != null)
                {
                    return View("~/Views/ErrorPage.cshtml", "Автор с этим именем уже существует");
                }
                else
                {
                    if (authorDTO != null)
                    {
                        authorService.CreateAuthor(authorDTO);
                    }
                }
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAuthor(AuthorViewModel model)
        {
            AuthorDTO? authorDTO = await authorService.GetAuthor(model.Id);
            if (authorDTO != null) 
            {
                if (authorDTO.Books != null && authorDTO.Books.Any())
                {
                    IEnumerable<BookDTO> bookDTOs = authorDTO.Books;
                    foreach (BookDTO bookDTO in bookDTOs)
                    {
                        bookDTO.AuthorId = Guid.Parse(UnknownAuthor.Id);
                        bookDTO.AuthorName = UnknownAuthor.Name;
                    }
                    bookService.UpdateBooksRange(bookDTOs);
                }
                authorService.DeleteAuthor(model.Id);
            }
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
    }
}
