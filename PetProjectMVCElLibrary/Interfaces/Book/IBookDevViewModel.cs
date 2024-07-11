using PetProjectMVCElLibrary.ViewModel.Author;
using PetProjectMVCElLibrary.ViewModel.Book;
using PetProjectMVCElLibrary.ViewModel.Genre;

namespace PetProjectMVCElLibrary.Interfaces.Book
{
    public interface IBookDevViewModel
    {
        IEnumerable<BookViewModel>? Books { get; set; }
        IEnumerable<GenreViewModel>? Genres { get; set; }
        IEnumerable<AuthorViewModel>? Authors { get; set; }
    }
}
