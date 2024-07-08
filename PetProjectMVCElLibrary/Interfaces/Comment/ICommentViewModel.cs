namespace PetProjectMVCElLibrary.Interfaces.Comment
{
    public interface ICommentViewModel
    {
        Guid Id { get; set; }
        Guid BookId { get; set; }
        string? Text { get; set; }
        DateTime? CreateOn { get; set; }
        string? UserName { get; set; }
        string? UserId { get; set; }
    }
}
