using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Repository
{
    /// <summary>
    /// Реализация репозитория для ApplicationUser
    /// </summary>
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly AppDbContext _context;
        public ApplicationUserRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Получаем список всех пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ApplicationUser>> GetAllEntityesAsync()
        {
            return await _context.ApplicationUsers.ToListAsync();
        }
        /// <summary>
        /// Получаем пользователя по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApplicationUser?> GetEntityByIdAsync(Guid id)
        {
            return await _context.ApplicationUsers
                .Include(x => x.Comments)
                .Include(x => x.Bookings)
                .FirstOrDefaultAsync(x => x.Id == id.ToString());
        }
        /// <summary>
        /// Добавляем пользователя в БД
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SaveEntity(ApplicationUser entity)
        {
            if (entity.Id == default)
            {
                _context.Add(entity);
            }
            else
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
            _context.SaveChanges();
        }
        /// <summary>
        /// Удаляем пользователя из бд
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void DeleteEntity(Guid id)
        {
            _context.ApplicationUsers.Remove(new ApplicationUser() { Id = id.ToString() });
            _context.SaveChanges();
        }
        /// <summary>
        /// Удаление пользователей из бд на основании массива пользователей
        /// </summary>
        /// <param name="entityes"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void DeleteRangeEntityes(IEnumerable<ApplicationUser> entityes)
        {
            _context.ApplicationUsers.RemoveRange(entityes);
            _context.SaveChanges();
        }

        public Task<ApplicationUser?> GetUserByEmail(string email)
        {
            return _context.ApplicationUsers.Where(x => x.NormalizedEmail == email.ToUpper()).FirstOrDefaultAsync();
        }
    }
}
