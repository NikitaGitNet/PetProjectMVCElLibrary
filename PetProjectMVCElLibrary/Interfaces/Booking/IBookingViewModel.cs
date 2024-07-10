namespace PetProjectMVCElLibrary.Interfaces.Booking
{
    public interface IBookingViewModel
    {
        Guid Id { get; set; }
        DateTime CreateOn { get; set; }
        DateTime FinishedOn { get; set; }
        bool IssueBooking { get; set; }
        string? UserEmail { get; set; }
        Guid BookId { get; set; }
        string? BooksTitle { get; set; }
    }
}
