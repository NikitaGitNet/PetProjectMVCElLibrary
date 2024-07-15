using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUserRepository(AppDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
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
                _context.ApplicationUsers.Add(entity);
            }
            else
            {
                _context.ApplicationUsers.Update(entity);
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

        public async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            return await _context.ApplicationUsers.Where(x => x.NormalizedEmail == email.ToUpper()).FirstOrDefaultAsync();
        }

        public async Task<bool> SignInResultSucceeded(string email, string password, bool rememberMe)
        {
            ApplicationUser? applicationUser = await GetUserByEmail(email);
            if (applicationUser != null)
            {
                SignInResult result = await _signInManager.PasswordSignInAsync(applicationUser, password, rememberMe, false);
                return result.Succeeded;
            }
            return false;
        }
        public async void ChangePassword(Guid userId, string password)
        {
            ApplicationUser? applicationUser = await GetEntityByIdAsync(userId);
            if (applicationUser != null)
            {
                applicationUser.PasswordHash = _userManager.PasswordHasher.HashPassword(applicationUser, password);
                await _userManager.UpdateAsync(applicationUser);
            }
        }
    }
}
