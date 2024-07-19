using BLL.Interfaces.DTO;

namespace BLL.Models.DTO.Booking
{
    /// <summary>
    /// ДТО брони
    /// </summary>
    public class BookingDTO : IBookingDTO
    {
        /// <summary>
        /// ИД брони
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreateOn { get; set; }
        /// <summary>
        /// Дата окончания брони
        /// </summary>
        public DateTime FinishedOn { get; set; }
        /// <summary>
        /// Флаг выдачи книги пользователю
        /// </summary>
        public bool IssueBooking { get; set; }
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// ИД пользователя
        /// </summary>
        public string? UserId { get; set; }
        /// <summary>
        /// ИД книги
        /// </summary>
        public Guid BookId { get; set; }
        /// <summary>
        /// Название книги
        /// </summary>
        public string? BooksTitle { get; set; }
        /// <summary>
        /// Уникальный код для получения книги
        /// </summary>
        public string? ReceiptCode { get; set; }
    }
}
