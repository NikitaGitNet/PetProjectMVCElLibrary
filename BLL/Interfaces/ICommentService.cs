using BLL.Models.DTO.Comment;

namespace BLL.Interfaces
{
    /// <summary>
    /// Интерфейс бизнес-логики взаимодействия с ДТО комментария
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// Получение книги из БД, маппинг в ДТО на основании ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CommentDTO> GetComment(Guid id);
        /// <summary>
        /// Получение комментов для конкретной книги
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<CommentDTO>> GetCommentsByBookId(Guid id);
        /// <summary>
        /// Получение всех комментов из БД, маппинг в ДТО
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CommentDTO>> GetAllComments();
        /// <summary>
        /// Сохранение коммента в БД на основе ДТО
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        void CreateComment(CommentDTO comment);
        /// <summary>
        /// Удаление книги из БД на основании Id
        /// </summary>
        /// <param name="id"></param>
        void DeleteComment(Guid bookId);
        /// <summary>
        /// Маппинг ДТО в ентити, удаление ентити из бд на основании массива полученных ентити
        /// </summary>
        /// <param name="entityes"></param>
        void DeleteRangeComments(IEnumerable<CommentDTO> commentDTOs);
    }
}
