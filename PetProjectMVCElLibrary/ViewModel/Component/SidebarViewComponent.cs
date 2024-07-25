using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.Book;
using BLL.Services.Book;
using DAL.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.ViewModel.Book;

namespace PetProjectMVCElLibrary.ViewModel.Component
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public SidebarViewComponent(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<BookDTO> bookDTOs = await _bookService.GetAllBooks();
            bookDTOs = bookDTOs.OrderBy(x => x.DateAdded);
            List<BookViewModel> booksViewModel = new();
            IEnumerable<BookViewModel> bookViewModels = _mapper.Map<IEnumerable<BookViewModel>>(bookDTOs);
            bookViewModels = bookViewModels.Take(5);
            return (IViewComponentResult)View("Default", bookViewModels);
        }
    }
}
