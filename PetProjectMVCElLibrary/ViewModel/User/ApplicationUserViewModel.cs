using PetProjectMVCElLibrary.Interfaces.User;
using PetProjectMVCElLibrary.ViewModel.Booking;
using PetProjectMVCElLibrary.ViewModel.Comment;

namespace PetProjectMVCElLibrary.ViewModel.User
{
    public class ApplicationUserViewModel : IApplicationUserViewModel
    {
        public ApplicationUserViewModel()
        {
            Bookings = new List<BookingViewModel>();
            Comments = new List<CommentViewModel>();
        }
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }
        public DateTime CreateOn { get; set; }
        public IEnumerable<BookingViewModel> Bookings { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
