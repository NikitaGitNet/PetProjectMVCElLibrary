using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        /// Метод проверки соответсвует ли текущий пользователь заявленной роли
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<bool> IsUserRoleConfirm(Guid userId, string roleName)
        { 
            bool confirm = false;
            IEnumerable<IdentityRole> roles = await _context.Roles.ToListAsync();
            IEnumerable<IdentityUserRole<string>> userRoles = await _context.IdentityUserRoles.ToListAsync();
            IdentityUserRole<string>? userRole = userRoles.FirstOrDefault(x => x.UserId == userId.ToString());
            if (userRole != null)
            {
                IdentityRole? role = roles.FirstOrDefault(x => x.Id == userRole.RoleId);
                if (role != null) 
                {
                    if (role.Name == roleName)
                    {
                        confirm = true;
                    }
                }
            }
            return confirm;
        }
        /// <summary>
        /// Добавляем пользователя в БД
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="password"></param>
        /// <param name="userManager"></param>
        /// <returns></returns>
        public async Task<bool> SaveUser(ApplicationUser entity, string password, UserManager<ApplicationUser> userManager)
        {
            bool result = false;
            if (password != default)
            {
                IdentityResult? identityResult = await userManager.CreateAsync(entity, password);
                if (identityResult != null)
                {
                    result = identityResult.Succeeded;
                    _context.SaveChanges();
                }
            }
            return result;
        }
        public async Task<bool> UpdateUser(ApplicationUser entity, string password, UserManager<ApplicationUser> userManager)
        {
            bool result = false;
            IdentityResult? identityResult = await userManager.UpdateAsync(entity);
            if (identityResult != null)
            {
                result = identityResult.Succeeded;
            }
            return result;
        }
        public void SaveEntity(ApplicationUser entity)
        {
            throw new NotImplementedException();
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
        /// <summary>
        /// Получение пользователя из БД на основании email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            return await _context.ApplicationUsers.Where(x => x.NormalizedEmail == email.ToUpper()).FirstOrDefaultAsync();
        } 
    }
}
