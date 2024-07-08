using BLL.Models.DTO.Book;
using BLL.Models.DTO.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICommentService
    {
        /// <summary>
        /// Сохранение коммента в БД на основе ДТО
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task CreateComment(CommentDTO comment);
        /// <summary>
        /// Получение книги из БД, маппинг в ДТО на основании ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CommentDTO> GetComment(Guid id);
        /// <summary>
        /// Получение всех комментов из БД, маппинг в ДТО
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CommentDTO>> GetAllComments();
        /// <summary>
        /// Маппинг ДТО в ентити, сохранение ентити в БД
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task SaveCommentAsync(CommentDTO commentDTO);
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
