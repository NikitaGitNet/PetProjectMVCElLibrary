using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository.Author;
using Microsoft.EntityFrameworkCore;

namespace DAL.Domain.Repository
{
    /// <summary>
    /// Реализация репозитория для энтити Автора
    /// </summary>
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;
        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Получение всех авторов из БД
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Author>> GetAllEntityesAsync()
        {
            return await _context.Authors.ToListAsync();
        }
        /// <summary>
        /// Получаем конкретного автора на основании ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Author?> GetEntityByIdAsync(Guid id)
        {
            return await _context.Authors.Include(x => x.Books).FirstOrDefaultAsync(x => x.Id == id);
        }
        /// <summary>
        /// Получаем автора на основании названия
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Author?> GetEntityByNameAsync(string name)
        {
            return await _context.Authors.FirstOrDefaultAsync(x => (x.Name ?? "").ToUpper() == name.ToUpper());
        }
        /// <summary>
        /// Сохраняем автора в БД
        /// </summary>
        /// <param name="entity"></param>
        public void SaveEntity(Author entity)
        {
            Author? author = _context.Authors.FirstOrDefault(x => x.Id == entity.Id);
            if (author != null) 
            {
                _context.Authors.Update(entity);
            }
            else
            {
                _context.Authors.Add(entity);
            }
            _context.SaveChanges();
        }
        /// <summary>
        /// Удаляем автора из БД на основании ИД
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEntity(Guid id)
        {
            _context.Authors.Remove(new Author() { Id = id });
            _context.SaveChanges();
        }
        /// <summary>
        /// Удаление нескольких авторов на основании Массива ентити
        /// </summary>
        /// <param name="entityes"></param>
        public void DeleteRangeEntityes(IEnumerable<Author> entityes)
        {
            _context.Authors.RemoveRange(entityes);
            _context.SaveChanges();
        }
    }
}
