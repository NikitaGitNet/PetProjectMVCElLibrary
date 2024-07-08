using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository;
using DAL.Domain.Interfaces.Repository.Comment;
using Microsoft.EntityFrameworkCore;

namespace DAL.Domain.Repository
{
    /// <summary>
    /// Методы взаимодействия с entity Comment
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Получение всех имеющихся комментариев
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>> GetAllEntityesAsync()
        {
            return await _context.Comments.ToListAsync();
        }
        /// <summary>
        /// Получение комментария по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Comment?> GetEntityByIdAsync(Guid id)
        {
            return _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
        }
        /// <summary>
        /// Сохранение комментария в БД
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task SaveEntityAsync(Comment entity)
        {
            if (entity.Id == default)
            {
                await _context.Comments.AddAsync(entity);
            }
            else
            {
                _context.Comments.Update(entity);
            }
            _context.SaveChanges();
        }
        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEntity(Guid id)
        {
            _context.Comments.Remove(new Comment() { Id = id });
            _context.SaveChanges();
        }
        /// <summary>
        /// Массовое удаление комментариев на основании коллекции
        /// </summary>
        /// <param name="entityes"></param>
        public void DeleteRangeEntityes(IEnumerable<Comment> entityes)
        {
            _context.Comments.RemoveRange(entityes);
            _context.SaveChanges();
        }
    }
}
