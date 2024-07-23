using PetProjectMVCElLibrary.Interfaces.Genre;
using PetProjectMVCElLibrary.ViewModel.Book;
using System.ComponentModel.DataAnnotations;

namespace PetProjectMVCElLibrary.ViewModel.Genre
{
    public class GenreViewModel : IGenreViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string? Name { get; set; }
        public IEnumerable<BookViewModel>? Books { get; set; }
    }
}
