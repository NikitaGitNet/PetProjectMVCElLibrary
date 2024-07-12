using PetProjectMVCElLibrary.ViewModel.Author;
using PetProjectMVCElLibrary.ViewModel.Genre;

namespace PetProjectMVCElLibrary.ViewModel.Book
{
    public class BookDevViewModel
    {
        public BookDevViewModel()
        {
            Books = new List<BookViewModel>();
            Genres = new List<GenreViewModel>();
            Authors = new List<AuthorViewModel>();
        }
        public IEnumerable<BookViewModel> Books { get; set; }
        public IEnumerable<GenreViewModel> Genres { get; set; }
        public IEnumerable<AuthorViewModel> Authors { get; set; }
    }
}
