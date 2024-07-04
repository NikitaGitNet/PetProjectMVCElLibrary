using AutoMapper;
using BLL.Interfaces;
using BLL.Interfaces.DTO;
using BLL.Models.DTO.Book;
using BLL.Services;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.Interfaces.Book;
using PetProjectMVCElLibrary.ViewModel.Book;

namespace PetProjectMVCElLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UnitOfWork unitOfWork;
        private readonly BookService bookService;
        private readonly IMapper mapper;
        public BookController(AppDbContext context)
        {
            _context = context;
            unitOfWork = new UnitOfWork(context);
            bookService = new BookService(context);
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<IBookDTO, IBookViewModel>()).CreateMapper();
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<IBookDTO> bookDTOs = await bookService.GetAllBooks();
            IEnumerable<IBookViewModel> bookViewModels = mapper.Map<IEnumerable<IBookDTO>, IEnumerable<IBookViewModel>>(bookDTOs);
            return View(bookViewModels);
        }
    }
}
