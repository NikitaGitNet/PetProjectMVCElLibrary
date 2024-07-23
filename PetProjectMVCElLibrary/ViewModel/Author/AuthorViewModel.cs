using PetProjectMVCElLibrary.Interfaces.Author;
using PetProjectMVCElLibrary.ViewModel.Book;
using System.ComponentModel.DataAnnotations;

namespace PetProjectMVCElLibrary.ViewModel.Author
{
    public class AuthorViewModel : IAuthorViewModel
    {
        public Guid Id { get; set; }
		[Required(ErrorMessage = "Поле не заполнено")]
		public string? Name { get; set; }
        public IEnumerable<BookViewModel>? Books { get; set; }
    }
}
