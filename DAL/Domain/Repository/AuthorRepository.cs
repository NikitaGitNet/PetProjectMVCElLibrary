using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository.Author;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
        }
        /// <summary>
        /// Сохраняем автора в БД
        /// </summary>
        /// <param name="entity"></param>
        public void SaveEntity(Author entity)
        {
            if (entity.Id == default)
            {
                _context.Authors.Add(entity);
            }
            else
            {
                _context.Authors.Update(entity);
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
