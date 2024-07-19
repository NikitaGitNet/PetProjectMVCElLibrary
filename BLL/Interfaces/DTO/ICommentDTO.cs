namespace BLL.Interfaces.DTO
{
    /// <summary>
    /// Интерфейс ДТО комментария
    /// </summary>
    public interface ICommentDTO
    {
        /// <summary>
        /// ИД комментария
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// Текст комментария
        /// </summary>
        string? CommentText { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        DateTime? CreateOn { get; set; }
        /// <summary>
        /// Почта пользователя
        /// </summary>
        string? Email { get; set; }
        /// <summary>
        /// Юзернейм пользователя
        /// </summary>
        string? UserName { get; set; }
        /// <summary>
        /// ИД пользователя
        /// </summary>
        string? UserId { get; set; }
        /// <summary>
        /// ИД книги
        /// </summary>
        Guid BookId { get; set; }
    }
}
