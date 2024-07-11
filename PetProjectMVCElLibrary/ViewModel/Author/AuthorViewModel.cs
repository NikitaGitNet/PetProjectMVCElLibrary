using PetProjectMVCElLibrary.Interfaces.Author;
using PetProjectMVCElLibrary.ViewModel.Book;

namespace PetProjectMVCElLibrary.ViewModel.Author
{
    public class AuthorViewModel : IAuthorViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<BookViewModel>? Books { get; set; }
    }
}
