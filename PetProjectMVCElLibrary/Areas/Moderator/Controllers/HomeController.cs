using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.Author;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Genre;
using BLL.Services.ApplicationUser;
using BLL.Services.Author;
using BLL.Services.Book;
using BLL.Services.Comment;
using BLL.Services.Genre;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.ViewModel.Author;
using PetProjectMVCElLibrary.ViewModel.Book;
using PetProjectMVCElLibrary.ViewModel.Genre;

namespace PetProjectMVCElLibrary.Areas.Moderator.Controllers
{
    [Area("Moderator")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBookService bookService;
        private readonly ICommentService commentService;
        private readonly IAuthorService authorService;
        private readonly IGenreService genreService;
        private readonly IApplicationUserService applicationUserService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _mapper = mapper;
            bookService = new BookService(context, mapper);
            commentService = new CommentService(context, mapper);
            authorService = new AuthorService(context, mapper);
            genreService = new GenreService(context, mapper);
            applicationUserService = new ApplicationUserService(context, signInManager, mapper);
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<BookDTO> bookDTOs = await bookService.GetAllBooks();
            bookDTOs = bookDTOs.OrderBy(x => x.Title);
            IEnumerable<BookViewModel> bookViewModels = _mapper.Map<IEnumerable<BookViewModel>>(bookDTOs);

            IEnumerable<GenreDTO> genreDTOs = await genreService.GetAllGenres();
            genreDTOs = genreDTOs.OrderBy(x => x.Name);
            IEnumerable<GenreViewModel> genreViewModels = _mapper.Map<IEnumerable<GenreViewModel>>(genreDTOs);

            IEnumerable<AuthorDTO> authorDTOs = await authorService.GetAllAuthors();
            authorDTOs = authorDTOs.OrderBy(x => x.Name);
            IEnumerable<AuthorViewModel> authorViewModels = _mapper.Map<IEnumerable<AuthorViewModel>>(authorDTOs);

            return View(new BookDevViewModel { Books = bookViewModels, Authors = authorViewModels, Genres = genreViewModels });
        }
    }
}
