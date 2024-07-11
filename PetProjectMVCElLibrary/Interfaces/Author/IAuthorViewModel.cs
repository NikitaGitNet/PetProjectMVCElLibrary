using BLL.Models.DTO.Book;
using PetProjectMVCElLibrary.ViewModel.Book;

namespace PetProjectMVCElLibrary.Interfaces.Author
{
    public interface IAuthorViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<BookViewModel>? Books { get; set; }
    }
}
