using PetProjectMVCElLibrary.Interfaces.Booking;

namespace PetProjectMVCElLibrary.ViewModel.Booking
{
    public class BookingViewModel : IBookingViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime FinishedOn { get; set; }
        public bool IssueBooking { get; set; }
        public string? UserEmail { get; set; }
        public Guid BookId { get; set; }
        public string? BooksTitle { get; set; }
        public string? ReceiptCode { get; set; }
    }
}
