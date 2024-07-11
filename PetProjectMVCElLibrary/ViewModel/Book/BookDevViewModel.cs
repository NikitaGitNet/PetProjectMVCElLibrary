using PetProjectMVCElLibrary.ViewModel.Author;
using PetProjectMVCElLibrary.ViewModel.Genre;

namespace PetProjectMVCElLibrary.ViewModel.Book
{
    public class BookDevViewModel
    {
        public IEnumerable<BookViewModel>? Books { get; set; }
        public IEnumerable<GenreViewModel>? Genres { get; set; }
        public IEnumerable<AuthorViewModel>? Authors { get; set; }
    }
}
