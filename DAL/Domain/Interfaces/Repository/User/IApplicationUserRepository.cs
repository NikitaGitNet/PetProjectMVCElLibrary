using DAL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Interfaces.Repository.User
{
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
    }
}
