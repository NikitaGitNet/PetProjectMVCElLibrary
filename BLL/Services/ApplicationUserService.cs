using AutoMapper;
using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Models.DTO.ApplicationUser;
using DAL.Domain;
using DAL.Domain.Entities;

namespace BLL.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly UnitOfWorkRepository Database;
        private readonly IMapper mapper;
        public ApplicationUserService(AppDbContext context)
        {
            Database = new UnitOfWorkRepository(context);
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, ApplicationUserDTO>()).CreateMapper();
        }
        public async Task<ApplicationUserDTO> GetUser(Guid id)
        {
            ApplicationUser? user = await Database.ApplicationUserRepository.GetEntityByIdAsync(id);
            if (user != null)
            {
                return mapper.Map<ApplicationUser, ApplicationUserDTO>(user);
            }
            // Если null, выбрасываем исключение
            throw new ValidationException("Пользователь не найден", "");
        }
        public async Task<ApplicationUserDTO> GetUserByEmail(string email)
        { 
            ApplicationUser user = await Database.ApplicationUserRepository.GetUserByEmail(email);
            if (user != null)
            {
                return mapper.Map<ApplicationUser, ApplicationUserDTO>(user);
            }
            throw new ValidationException("Пользователь не найден", "");
        }
        public async Task<IEnumerable<ApplicationUserDTO>> GetAllUsers()
        {
            IEnumerable<ApplicationUser> users = await Database.ApplicationUserRepository.GetAllEntityesAsync();
            return mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserDTO>>(users);
        }
        public async Task CreateUser(ApplicationUserDTO user)
        {
            ApplicationUser applicationUser = mapper.Map<ApplicationUserDTO , ApplicationUser>(user);
            await Database.ApplicationUserRepository.SaveEntityAsync(applicationUser);
        }
        public void DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }
        public void DeleteRangeUsers(IEnumerable<ApplicationUserDTO> users)
        {
            throw new NotImplementedException();
        }

        

        

       
    }
}
