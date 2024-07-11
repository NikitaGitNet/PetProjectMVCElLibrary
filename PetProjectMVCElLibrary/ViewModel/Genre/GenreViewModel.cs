using PetProjectMVCElLibrary.Interfaces.Genre;
using PetProjectMVCElLibrary.ViewModel.Book;

namespace PetProjectMVCElLibrary.ViewModel.Genre
{
    public class GenreViewModel : IGenreViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<BookViewModel>? Books { get; set; }
    }
}
