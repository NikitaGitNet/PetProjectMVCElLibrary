using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.Author;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Genre;
using BLL.Services;
using DAL.Domain;
using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository.Book;
using DAL.Domain.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Controllers;
using PetProjectMVCElLibrary.Service;
using PetProjectMVCElLibrary.ViewModel.Book;

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
        public BookController(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment hostingEnviroment)
        {
            _context = context;
            _mapper = mapper;
            bookService = new BookService(context, mapper);
            commentService = new CommentService(context, mapper);
            authorService = new AuthorService(context, mapper);
            genreService = new GenreService(context, mapper);
            applicationUserService = new ApplicationUserService(context, signInManager, mapper);
            _httpContextAccessor = httpContextAccessor;
            _hostingEnviroment = hostingEnviroment;
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            BookDTO entity = id == default ? new BookDTO() : await bookService.GetBook(id);
            return View(entity);
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
                    IEnumerable<AuthorDTO> authors = await authorService.GetAllAuthors();
                    authors = authors.Where(x => (x.Name ?? "").ToUpper() == bookDTO.AuthorName);
                    if (!authors.Any())
                    {
                        AuthorDTO authorDTO = new() { Name = bookDTO.AuthorName, Id = new Guid() };
                        authorService.CreateAuthor(authorDTO);
                        bookDTO.AuthorName = authorDTO.Name;
                        bookDTO.AuthorId = authorDTO.Id;
                    }
                }
                else
                {
                    bookDTO.AuthorName = UnknownAuthor.Name;
                    bookDTO.AuthorId = new Guid(UnknownAuthor.Id);
                }
                if (bookDTO.GenreName != null)
                {
                    IEnumerable<GenreDTO> genreDTOs = await genreService.GetAllGenres();
                    genreDTOs = genreDTOs.Where(x => (x.Name ?? "").ToUpper() == bookDTO.GenreName);
                    if (!genreDTOs.Any())
                    {
                        GenreDTO genreDTO = new() { Name = bookDTO.GenreName, Id = new Guid() };
                        genreService.CreateGenre(genreDTO);
                        bookDTO.GenreName = genreDTO.Name;
                        bookDTO.GenreId = genreDTO.Id;
                    }
                    else
                    {
                        bookDTO.GenreName = UnknownGenre.Name;
                        bookDTO.GenreId = new Guid(UnknownGenre.Id);
                    }
                    bookDTO.DateAdded = DateTime.Now;
                    bookService.CreateBook(bookDTO);
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
                }
                return View(_mapper.Map<BookViewModel>(bookDTO));
            }
            return View(bookViewModel);
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
    }
}
