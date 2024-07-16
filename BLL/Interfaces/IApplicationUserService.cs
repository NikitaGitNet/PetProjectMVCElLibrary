using BLL.Models.DTO.ApplicationUser;
using BLL.Models.DTO.Book;

namespace BLL.Interfaces
{
    public interface IApplicationUserService
    {
        Task<ApplicationUserDTO> GetUser(Guid id);
        Task<ApplicationUserDTO> GetUserByEmail(string email);
        Task<IEnumerable<ApplicationUserDTO>> GetAllUsers();
        Task<bool> SignInResultSucceeded(string email, string password, bool rememberMe);
        void CreateUser(ApplicationUserDTO user);
        Task<bool> ChangePassword(Guid userId, string password);
        void DeleteUser(Guid userId);
        void DeleteRangeUsers(IEnumerable<ApplicationUserDTO> users);
    }
}
