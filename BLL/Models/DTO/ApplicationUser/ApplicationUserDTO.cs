using BLL.Interfaces.DTO;
using BLL.Models.DTO.Booking;
using BLL.Models.DTO.Comment;

namespace BLL.Models.DTO.ApplicationUser
{
    /// <summary>
    /// ДТО юзера
    /// </summary>
    public class ApplicationUserDTO : IApplicationUserDTO
    {
        public ApplicationUserDTO()
        {
            Comments = new List<CommentDTO>();
            Bookings = new List<BookingDTO>();
        }
        /// <summary>
        /// ИД пользователя
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// Юзернейм пользователя
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// Дата регистрации пользователя
        /// </summary>
        public DateTime CreateOn { get; set; }
        /// <summary>
        /// ДТО связанных комментариев
        /// </summary>
        public ICollection<CommentDTO> Comments { get; set; }
        /// <summary>
        /// ДТО связанных броней
        /// </summary>
        public IEnumerable<BookingDTO> Bookings { get; set; }
    }
}
