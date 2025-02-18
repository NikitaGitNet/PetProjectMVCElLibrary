﻿using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository.Genre;
using Microsoft.EntityFrameworkCore;

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
            return await _context.Genres.Include(x => x.Books).FirstOrDefaultAsync(x => x.Id == id);
        }
        /// <summary>
        /// Получаем жанр из БД на основании названия жанра
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Genre?> GetEntityByNameAsync(string name)
        {
            return await _context.Genres.FirstOrDefaultAsync(x => (x.Name ?? "").ToUpper() == name.ToUpper());
        }
        /// <summary>
        /// Сохраняем в БД новый жанр
        /// </summary>
        /// <param name="entity"></param>
        public bool SaveEntity(Genre entity)
        {
            bool result = false;
            try
            {
                Genre? genre = _context.Genres.FirstOrDefault(x => x.Id == entity.Id);
                if (genre == null)
                {
                    _context.Genres.Add(entity);
                    _context.SaveChanges();
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// Обновляем в БД новый жанр
        /// </summary>
        /// <param name="entity"></param>
        public bool UpdateEntity(Genre entity)
        {
            bool result = false;
            try
            {
                Genre? genre = _context.Genres.FirstOrDefault(x => x.Id == entity.Id);
                if (genre != null)
                {
                    _context.Genres.Update(entity);
                    _context.SaveChanges();
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
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
