using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Comment;
using BLL.Services;
using DAL.Domain;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.ViewModel.Book;
using PetProjectMVCElLibrary.ViewModel.Comment;

namespace PetProjectMVCElLibrary.Controllers
{
    public class CommentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICommentService bookService;
        private readonly IMapper mapper;
        public CommentController(AppDbContext context)
        {
            _context = context;
            bookService = new CommentService(context);
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<CommentDTO, CommentViewModel>()).CreateMapper();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(string commentText)
        {
            CommentDTO commentDTO = new CommentDTO()
            { 
                Id = Guid.NewGuid(),
                CreateOn = DateTime.Now,
                Text = commentText,
                UserId = "5e84bf2c-585f-42dc-a868-73157016ec70",
                BookId = Guid.Parse("19f4d611-5a6a-4cf1-bcde-02e85081bb18"),
                UserEmail = "moderator@email.com",
                UserName = "moderator"
            };
            await bookService.SaveCommentAsync(commentDTO);
            return Ok();
        }
    }
}
