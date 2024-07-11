using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository.Genre;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Repository
{
    /// <summary>
    /// Реализация репозитория для энтити Жанра
    /// </summary>
    public class GenreRepository : IGenreRepository
    {
        private readonly AppDbContext _context;
        public GenreRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Получаем из БД все доступные жанры
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Genre>> GetAllEntityesAsync()
        {
            return await _context.Genres.ToListAsync();
        }
        /// <summary>
        /// Получаем конкретный жанр
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Genre?> GetEntityByIdAsync(Guid id)
        {
            return await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);
        }
        /// <summary>
        /// Сохраняем в БД новый жанр
        /// </summary>
        /// <param name="entity"></param>
        public void SaveEntity(Genre entity)
        {
            if (entity.Id == default)
            {
                _context.Genres.Add(entity);
            }
            else
            {
                _context.Genres.Update(entity);
            }
            _context.SaveChanges();
        }
        /// <summary>
        /// Удаляем жанр
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEntity(Guid id)
        {
            _context.Genres.Remove(new Genre() { Id = id });
            _context.SaveChanges();
        }
        /// <summary>
        /// Удаление жанров из бд на основании массива ентити
        /// </summary>
        /// <param name="entityes"></param>
        public void DeleteRangeEntityes(IEnumerable<Genre> entityes)
        {
            _context.Genres.RemoveRange(entityes);
            _context.SaveChanges();
        }
    }
}
