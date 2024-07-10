using BLL.Interfaces.DTO;
using BLL.Models.DTO.Booking;
using BLL.Models.DTO.Comment;

namespace BLL.Models.DTO.ApplicationUser
{
    public class ApplicationUserDTO : IApplicationUserDTO
    {
        public ApplicationUserDTO()
        {
            Comments = new List<CommentDTO>();
            Bookings = new List<BookingDTO>();
            Id = "";
            UserEmail = "";
            UserName = "";
        }
        public string Id { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public ICollection<CommentDTO> Comments { get; set; }
        public IEnumerable<BookingDTO> Bookings { get; set; }
    }
}
