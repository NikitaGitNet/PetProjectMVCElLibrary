using BLL.Models.DTO.Booking;
using BLL.Models.DTO.Comment;

namespace BLL.Interfaces.DTO
{
    /// <summary>
    /// Интерфейс ДТО юзера
    /// </summary>
    internal interface IApplicationUserDTO
    {
        /// <summary>
        /// ИД пользователя
        /// </summary>
        string? Id { get; set; }
        /// <summary>
        /// Почта пользователя
        /// </summary>
        string? Email { get; set; }
        /// <summary>
        /// Юзернейм пользователя
        /// </summary>
        string? UserName { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        string? Password { get; set; }
        /// <summary>
        /// Дата регистрации пользователя
        /// </summary>
        DateTime CreateOn { get; set; }
        /// <summary>
        /// ДТО связанных комментариев
        /// </summary>
        ICollection<CommentDTO> Comments { get; set; }
        /// <summary>
        /// ДТО связанных броней
        /// </summary>
        IEnumerable<BookingDTO> Bookings { get; set; }
    }
}
