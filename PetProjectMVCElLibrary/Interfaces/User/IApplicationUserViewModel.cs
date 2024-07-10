using PetProjectMVCElLibrary.Interfaces.Comment;
using PetProjectMVCElLibrary.ViewModel.Booking;
using PetProjectMVCElLibrary.ViewModel.Comment;

namespace PetProjectMVCElLibrary.Interfaces.User
{
    public interface IApplicationUserViewModel
    {
        string? Id { get; set; }
        string? UserEmail { get; set; }
        string? UserName { get; set; }
        DateTime CreateOn { get; set; }
        IEnumerable<BookingViewModel> Bookings { get; set; }
        IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
