using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.Book;
using BLL.Services;
using DAL.Domain;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Interfaces.Book;
using PetProjectMVCElLibrary.ViewModel.Book;

namespace PetProjectMVCElLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext _context;
        //private readonly UnitOfWork unitOfWork;
        private readonly IBookService bookService;
        private readonly IMapper mapper;
        //private readonly ILogger _logger;
        public BookController(AppDbContext context /*ILogger logger*/)
        {
            _context = context;
            //unitOfWork = new UnitOfWork(context);
            bookService = new BookService(context);
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<BookDTO, BookViewModel>()).CreateMapper();
            //_logger = logger;
        }
        /// <summary>
        /// Вывести все книги на экран
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<BookViewModel> bookViewModels = new List<BookViewModel>();
            try
            {
                IEnumerable<BookDTO> bookDTOs = await bookService.GetAllBooks();
                bookViewModels = mapper.Map<IEnumerable<BookDTO>, IEnumerable<BookViewModel>>(bookDTOs);
            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
                throw;
            }
            return View(bookViewModels);
        }
        [HttpGet]
        public async Task<IActionResult> ShowCurrentBook(BookViewModel book)
        {
            IBookViewModel bookViewModel = new BookViewModel();
            try
            {
                BookDTO bookDTO = await bookService.GetBook(book.Id);
                bookViewModel = mapper.Map<BookDTO, BookViewModel>(bookDTO);
            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
                throw;
            }
            return View(bookViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> SearchBookByAutor(Guid authorId)
        {
            IEnumerable<BookViewModel> bookViewModels = new List<BookViewModel>();
            try
            {
                IEnumerable<BookDTO> bookDTOs = await bookService.GetBookByAuthor(authorId);
                bookViewModels = mapper.Map<IEnumerable<BookDTO>, IEnumerable<BookViewModel>>(bookDTOs);
            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
                throw;
            }
            return View(bookViewModels);
        }
        [HttpGet]
        public async Task<IActionResult> SearchBookByGenre(Guid genreId)
        {
            IEnumerable<BookViewModel> bookViewModels = new List<BookViewModel>();
            try
            {
                IEnumerable<BookDTO> bookDTOs = await bookService.GetBookByGenre(genreId);
                bookViewModels = mapper.Map<IEnumerable<BookDTO>, IEnumerable<BookViewModel>>(bookDTOs);
            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
                throw;
            }
            return View(bookViewModels);
        }
    }
}
