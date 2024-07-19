namespace BLL.Interfaces.DTO
{
    /// <summary>
    /// Интерфейс ДТО брони
    /// </summary>
    public interface IBookingDTO
    {
        /// <summary>
        /// ИД брони
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        DateTime CreateOn { get; set; }
        /// <summary>
        /// Дата окончания брони
        /// </summary>
        DateTime FinishedOn { get; set; }
        /// <summary>
        /// Флаг выдачи книги пользователю
        /// </summary>
        bool IssueBooking { get; set; }
        /// <summary>
        /// Почта пользователя
        /// </summary>
        string? Email { get; set; }
        /// <summary>
        /// ИД пользователя
        /// </summary>
        string? UserId { get; set; }
        /// <summary>
        /// ИД книги
        /// </summary>
        Guid BookId { get; set; }
        /// <summary>
        /// Название книги
        /// </summary>
        string? BooksTitle { get; set; }
        /// <summary>
        /// Уникальный код для получения книги
        /// </summary>
        string? ReceiptCode { get; set; }
    }
}
