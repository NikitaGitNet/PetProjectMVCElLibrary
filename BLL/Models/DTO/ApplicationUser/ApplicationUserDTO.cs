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
        }
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public DateTime CreateOn { get; set; }
        public ICollection<CommentDTO> Comments { get; set; }
        public IEnumerable<BookingDTO> Bookings { get; set; }
    }
}
