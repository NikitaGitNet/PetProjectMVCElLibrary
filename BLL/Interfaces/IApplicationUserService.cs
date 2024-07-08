using BLL.Models.DTO.ApplicationUser;
using BLL.Models.DTO.Book;

namespace BLL.Interfaces
{
    public interface IApplicationUserService
    {
        Task<ApplicationUserDTO> GetUser(Guid id);
        Task<ApplicationUserDTO> GetUserByEmail(string email);
        Task<IEnumerable<ApplicationUserDTO>> GetAllUsers();
        Task CreateUser(ApplicationUserDTO user);
        void DeleteUser(string userId);
        void DeleteRangeUsers(IEnumerable<ApplicationUserDTO> users);
    }
}
