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
        Task<ApplicationUser?> GetUserByEmail(string email);
        Task<bool> SignInResultSucceeded(string email, string password, bool rememberMe);
        void ChangePassword(Guid userId, string password);
    }
}
