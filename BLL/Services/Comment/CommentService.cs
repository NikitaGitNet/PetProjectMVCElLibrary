using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.Comment;
using DAL.Domain;

namespace BLL.Services.Comment
{
    /// <summary>
    /// Бизнес-логика взаимодействия с ДТО комментария
    /// </summary>
    public class CommentService : ICommentService
    {
        private readonly UnitOfWorkRepository Database;
        private readonly IMapper _mapper;
        AppDbContext dbContext;
        public CommentService(AppDbContext context, IMapper mapper)
        {
            dbContext = context;
            Database = new UnitOfWorkRepository(context);
            _mapper = mapper;
        }
        /// <summary>
        /// Получение коммента из БД, маппинг в ДТО
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CommentDTO> GetComment(Guid id)
        {
            DAL.Domain.Entities.Comment? comment = await Database.CommentRepository.GetEntityByIdAsync(id);
            return _mapper.Map<CommentDTO>(comment);
        }
        /// <summary>
        /// Получение комментов для конкретной книги и маппинг их в ДТО
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CommentDTO>> GetCommentsByBookId(Guid id)
        {
            IEnumerable<DAL.Domain.Entities.Comment> comments = await Database.CommentRepository.GetEntityesByBookIdAsync(id);
            return _mapper.Map<IEnumerable<CommentDTO>>(comments);
        }
        /// <summary>
        /// Получение всех комментов из бд и маппинг их в ДТО
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommentDTO>> GetAllComments()
        {
            IEnumerable<DAL.Domain.Entities.Comment> comments = await Database.CommentRepository.GetAllEntityesAsync();
            return _mapper.Map<IEnumerable<CommentDTO>>(comments);
        }
        /// <summary>
        /// Маппинг дто в энтити и сохранение в БД
        /// </summary>
        /// <param name="commentDTO"></param>
        /// <returns></returns>
        public bool CreateComment(CommentDTO commentDTO)
        {
            DAL.Domain.Entities.Comment comment = _mapper.Map<DAL.Domain.Entities.Comment>(commentDTO);
            return Database.CommentRepository.SaveEntity(comment);
        }
        /// <summary>
        /// Маппинг дто в энтити и обновление в БД
        /// </summary>
        /// <param name="commentDTO"></param>
        /// <returns></returns>
        public bool UpdateComment(CommentDTO commentDTO)
        {
            DAL.Domain.Entities.Comment comment = _mapper.Map<DAL.Domain.Entities.Comment>(commentDTO);
            return Database.CommentRepository.SaveEntity(comment);
        }
        /// <summary>
        /// Удаление коммента из БД
        /// </summary>
        /// <param name="bookId"></param>
        public void DeleteComment(Guid bookId)
        {
            Database.CommentRepository.DeleteEntity(bookId);
        }
        /// <summary>
        /// Массовое удаление комментариев на основании массива ДТО
        /// Мапим массив ДТО в массив энтити, передаем на слой ниже, удаляем
        /// </summary>
        /// <param name="commentDTOs"></param>
        public void DeleteRangeComments(IEnumerable<CommentDTO> commentDTOs)
        {
            IEnumerable<DAL.Domain.Entities.Comment> comments = _mapper.Map<IEnumerable<DAL.Domain.Entities.Comment>>(commentDTOs);
            Database.CommentRepository.DeleteRangeEntityes(comments);
        }
    }
}
