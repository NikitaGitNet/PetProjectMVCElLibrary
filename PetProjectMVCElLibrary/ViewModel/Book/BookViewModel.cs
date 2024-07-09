using PetProjectMVCElLibrary.Interfaces.Book;
using PetProjectMVCElLibrary.Interfaces.Comment;
using PetProjectMVCElLibrary.ViewModel.Comment;
using System.ComponentModel.DataAnnotations;

namespace PetProjectMVCElLibrary.ViewModel.Book
{
    public class BookViewModel : IBookViewModel
    {
        public BookViewModel()
        {
            Comments = new List<CommentViewModel>();
        }
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Заполните название книги")]
        [Display(Name = "Название книги")]
        public string? Title { get; set; }
        [Display(Name = "Краткое описание книги")]
        public string? SubTitle { get; set; }
        [Display(Name = "Полное описание книги")]
        public string? AuthorName { get; set; }
        [Display(Name = "Автор")]
        public string? GenreName { get; set; }
        [Display(Name = "Жанр")]
        public string? TitleImagePath { get; set; }
        public string? Text { get; set; }
        public bool IsBooking { get; set; }
        public string? CommentText { get; set; }
        public DateTime DateAdded { get; set; }
        public string? CurentUserId { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
