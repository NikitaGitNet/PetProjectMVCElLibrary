using DAL.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Interfaces.Repository.User
{
    /// <summary>
    /// Интерфейс расширяющий логику взаимодействия с энтити пользователя
    /// </summary>
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        /// <summary>
        /// Получение пользователя из БД на основании email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<ApplicationUser?> GetUserByEmail(string email);
        /// <summary>
        /// Метод проверки соответсвует ли текущий пользователь заявленной роли
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<bool> IsUserRoleConfirm(Guid userId, string roleName);
        /// <summary>
        /// Добавляем пользователя в БД
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="password"></param>
        /// <param name="userManager"></param>
        /// <returns></returns>
        Task<bool> SaveUser(ApplicationUser entity, string password, UserManager<ApplicationUser> userManager);
    }
}
