using PetProjectMVCElLibrary.Interfaces.Comment;

namespace PetProjectMVCElLibrary.ViewModel.Comment
{
    public class CommentViewModel : ICommentViewModel
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string? CommentText { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? UserName { get; set; }
        public string? UserId { get; set; }
    }
}
