using PetProjectMVCElLibrary.ViewModel.Book;

namespace PetProjectMVCElLibrary.Interfaces.Genre
{
    public interface IGenreViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<BookViewModel>? Books { get; set; }
    }
}
