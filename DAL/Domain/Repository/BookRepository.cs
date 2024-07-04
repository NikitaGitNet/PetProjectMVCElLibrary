using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Domain.Repository
{
    /// <summary>
    /// Методы взаимодействия с entity Book
    /// </summary>
    public class BookRepository : IRepository<Book>
    {
        private readonly AppDbContext _context;
        public BookRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Получение всех имеющихся entityes из БД
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Book>> GetAllEntityesAsync()
        {
            return await _context.Books.ToListAsync();
        }
        /// <summary>
        /// Получение entity по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Book?> GetEntityByIdAsync(Guid id)
        {
            return await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
        }
        /// <summary>
        /// Получение entityes из БД по массиву идентификаторов
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Book>> GetEntityesByIdsAsync(IEnumerable<Guid> ids)
        {
            return await _context.Books.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
        /// <summary>
        /// Сохранение entity в БД
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task SaveEntityAsync(Book entity)
        {
            await _context.Books.AddAsync(entity);
            _context.SaveChanges();
        }
        /// <summary>
        /// Сохрание коллекции entity в БД
        /// </summary>
        /// <param name="entityes"></param>
        /// <returns></returns>
        public async Task SaveRangeEntityesAsync(IEnumerable<Book> entityes)
        {
            await _context.Books.AddRangeAsync(entityes);
            _context.SaveChanges();
        }
        /// <summary>
        /// Удаление по идентификатору
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEntity(Guid id)
        {
            _context.Books.Remove(new Book() { Id = id });
            _context.SaveChanges();
        }
        /// <summary>
        /// Массовое удаление по массиву entityes
        /// </summary>
        /// <param name="entityes"></param>
        public void DeleteRangeEntityes(IEnumerable<Book> entityes)
        {
            _context.Books.RemoveRange(entityes);
            _context.SaveChanges();
        }
    }
}
