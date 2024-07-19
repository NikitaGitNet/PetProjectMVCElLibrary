using BLL.Interfaces.DTO;

namespace BLL.Models.DTO.Comment
{
    /// <summary>
    /// ДТО комментария
    /// </summary>
    public class CommentDTO : ICommentDTO
    {
        /// <summary>
        /// ИД комментария
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Текст комментария
        /// </summary>
        public string? CommentText { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime? CreateOn { get; set; }
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Юзернейм пользователя
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// ИД пользователя
        /// </summary>
        public string? UserId { get; set; }
        /// <summary>
        /// ИД книги
        /// </summary>
        public Guid BookId { get; set; }
    }
}
