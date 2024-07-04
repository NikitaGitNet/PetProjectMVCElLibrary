using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Domain.Repository
{
    /// <summary>
    /// Методы взаимодействия с entity Comment
    /// </summary>
    public class CommentRepository : IRepository<Comment>
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
        /// Получение комментариев по коллекции Id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>> GetEntityesByIdsAsync(IEnumerable<Guid> ids)
        {
            return await _context.Comments.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
        /// <summary>
        /// Сохранение комментария в БД
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task SaveEntityAsync(Comment entity)
        {
            await _context.Comments.AddAsync(entity);
            _context.SaveChanges();
        }
        /// <summary>
        /// Сохранение коллекции коментариев
        /// </summary>
        /// <param name="entityes"></param>
        /// <returns></returns>
        public async Task SaveRangeEntityesAsync(IEnumerable<Comment> entityes)
        {
            await _context.Comments.AddRangeAsync(entityes);
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
