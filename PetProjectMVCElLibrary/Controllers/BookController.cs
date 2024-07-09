using AutoMapper;
using BLL.Interfaces;
using BLL.Interfaces.DTO;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Comment;
using BLL.Services;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Interfaces.Book;
using PetProjectMVCElLibrary.ViewModel.Book;
using PetProjectMVCElLibrary.ViewModel.Comment;
using System.Security.Claims;

namespace PetProjectMVCElLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBookService bookService;
        private readonly ICommentService commentService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BookController(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            bookService = new BookService(context, mapper);
            commentService = new CommentService(context, mapper);
            _httpContextAccessor = httpContextAccessor;
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
                bookViewModels = _mapper.Map<IEnumerable<BookViewModel>>(bookDTOs);
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
                bookViewModel = _mapper.Map<BookViewModel>(bookDTO);
                IEnumerable<CommentDTO> commentDTOs = await commentService.GetCommentsByBookId(book.Id);
                bookViewModel.Comments = _mapper.Map<IEnumerable<CommentViewModel>>(commentDTOs);
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
                bookViewModels = _mapper.Map<IEnumerable<BookDTO>, IEnumerable<BookViewModel>>(bookDTOs);
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
                bookViewModels = _mapper.Map<IEnumerable<BookDTO>, IEnumerable<BookViewModel>>(bookDTOs);
            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
                throw;
            }
            return View(bookViewModels);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel commentViewModel)
        {
            HttpContext? httpContent = _httpContextAccessor.HttpContext;
            if (httpContent != null)
            {
                Claim? claim = httpContent.User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    var userId = claim.Value;

                    return Ok();
                }
                CommentDTO commentDTO = new CommentDTO()
                {
                    CreateOn = DateTime.Now,
                    CommentText = commentViewModel.CommentText,
                    UserId = "86d55f40-9544-4d92-aa24-cc5693a5fd96",
                    BookId = commentViewModel.Id,
                    UserEmail = "moderator@email.com",
                    UserName = "moderator"
                };
                commentService.CreateComment(commentDTO);
            }
            BookDTO bookDTO = await bookService.GetBook(commentViewModel.Id);
            return View("ShowCurrentBook", _mapper.Map<BookViewModel>(bookDTO));
        }
    }
}
