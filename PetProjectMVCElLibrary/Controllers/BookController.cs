using AutoMapper;
using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Interfaces.DTO;
using BLL.Models.DTO.ApplicationUser;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Comment;
using BLL.Services;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Interfaces.Book;
using PetProjectMVCElLibrary.Interfaces.Comment;
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
        private readonly IApplicationUserService applicationUserService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BookController(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _mapper = mapper;
            bookService = new BookService(context, mapper);
            commentService = new CommentService(context, mapper);
            applicationUserService = new ApplicationUserService(context, signInManager, mapper);
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
                HttpContext? httpContent = _httpContextAccessor.HttpContext;
                if (httpContent != null)
                {
                    Claim? claim = httpContent.User.FindFirst(ClaimTypes.NameIdentifier);
                    if (claim != null)
                    {
                        var userId = claim.Value;
                        bookViewModel.CurentUserId = userId;
                    }
                }
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
            BookDTO bookDTO = await bookService.GetBook(commentViewModel.Id);
            if (bookDTO != null)
            {
                if (httpContent != null)
                {
                    Claim? claim = httpContent.User.FindFirst(ClaimTypes.NameIdentifier);
                    if (claim != null)
                    {
                        var userId = claim.Value;
                        ApplicationUserDTO userDTO = await applicationUserService.GetUser(userId);
                        CommentDTO commentDTO = new CommentDTO()
                        {
                            CreateOn = DateTime.Now,
                            CommentText = commentViewModel.CommentText,
                            UserId = userId,
                            BookId = commentViewModel.Id,
                            UserEmail = userDTO.UserEmail,
                            UserName = userDTO.UserName,
                        };
                        commentService.CreateComment(commentDTO);
                        //return View("ShowCurrentBook", _mapper.Map<BookViewModel>(bookDTO));
                        return RedirectToAction(nameof(BookController.ShowCurrentBook), _mapper.Map<BookViewModel>(bookDTO));
                    }
                }
                return View("ShowCurrentBook", _mapper.Map<BookViewModel>(bookDTO));
            }
            throw new ValidationException("Книга не найдена", "");
        }
        public async Task<IActionResult> DeleteComment(CommentViewModel model)
        {
            commentService.DeleteComment(model.Id);
            BookDTO bookDTO = await bookService.GetBook(model.BookId);
            return RedirectToAction(nameof(BookController.ShowCurrentBook), _mapper.Map<BookViewModel>(bookDTO));
        }
    }
}
