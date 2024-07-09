using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Comment;
using BLL.Services;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProjectMVCElLibrary.Service;
using PetProjectMVCElLibrary.ViewModel.Book;
using PetProjectMVCElLibrary.ViewModel.Comment;
using System.Security.Claims;

namespace PetProjectMVCElLibrary.Controllers
{
    public class CommentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICommentService commentService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommentController(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            commentService = new CommentService(context, mapper);
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(string commentText)
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
                    CommentText = "dsfsdfsdfsd",
                    UserId = "5e84bf2c-585f-42dc-a868-73157016ec70",
                    BookId = Guid.Parse("19f4d611-5a6a-4cf1-bcde-02e85081bb18"),
                    UserEmail = "moderator@email.com",
                    UserName = "moderator"
                };
                Comment comment = _mapper.Map<Comment>(commentDTO);
                commentService.CreateComment(commentDTO);

            }
            return Ok();
            
        }
    }
}
