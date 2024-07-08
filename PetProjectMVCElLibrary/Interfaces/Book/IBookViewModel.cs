using BLL.Interfaces.DTO;
using PetProjectMVCElLibrary.Interfaces.Comment;
using PetProjectMVCElLibrary.ViewModel.Comment;
using System.ComponentModel.DataAnnotations;

namespace PetProjectMVCElLibrary.Interfaces.Book
{
    public interface IBookViewModel
    {
        Guid Id { get; set; }
        string? Title { get; set; }
        string? SubTitle { get; set; }
        string? AuthorName { get; set; }
        string? GenreName { get; set; }
        string? TitleImagePath { get; set; }
        string? CurentUserId { get; set; }
        string? Text { get; set; }
        bool IsBooking { get; set; }
        string? CommentText { get; set; }
        DateTime DateAdded { get; set; }
        IEnumerable<ICommentViewModel> Comments { get; set; }
    }
}
