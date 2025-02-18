﻿using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository.Book;
using Microsoft.EntityFrameworkCore;

namespace DAL.Domain.Repository
{
    /// <summary>
    /// Методы взаимодействия с entity Book
    /// </summary>
    public class BookRepository : IBookRepository
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
            return await _context.Books
                .Include(x => x.Author)
                .Include(x => x.Genre)
                .Include(x => x.Bookings)
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        /// <summary>
        /// Получение книг по жанру
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Book>> GetEntityesByGenreAsync(Guid genreId)
        { 
            return await _context.Books.Where(x => x.GenreId == genreId).ToListAsync();
        }
        /// <summary>
        /// Получение книг по автору
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Book>> GetEntityesByAuthorAsync(Guid authorId)
        {
            return await _context.Books.Where(x => x.AuthorId == authorId).ToListAsync();
        }
        /// <summary>
        /// Сохранение entity в БД
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SaveEntity(Book entity)
        {
            bool result = false;
            try
            {
                Book? book = _context.Books.FirstOrDefault(x => x.Id == entity.Id);
                if (book == null)
                {
                    _context.Books.Add(entity);
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
        /// Обновление энтити в БД
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateEntity(Book entity)
        {
            bool result = false;
            try
            {
                Book? book = _context.Books.FirstOrDefault(x => x.Id == entity.Id);
                if (book != null)
                {
                    _context.Books.Update(entity);
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
        /// Обновление книг на основании массива ентити
        /// </summary>
        /// <param name="books"></param>
        public void UpdateEntityRange(IEnumerable<Book> books)
        {
            if (books.Any())
            {
                _context.UpdateRange(books);
                _context.SaveChanges();
            }
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
