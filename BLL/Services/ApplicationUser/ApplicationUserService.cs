using AutoMapper;
using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Models.DTO.ApplicationUser;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services.ApplicationUser
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly UnitOfWorkRepository Database;
        private readonly IMapper _mapper;
        private readonly SignInManager<DAL.Domain.Entities.ApplicationUser> _signInManager;
        public ApplicationUserService(AppDbContext context, SignInManager<DAL.Domain.Entities.ApplicationUser> signInManager, IMapper mapper)
        {
            Database = new UnitOfWorkRepository(context);
            _mapper = mapper;
            _signInManager = signInManager;
        }
        public async Task<ApplicationUserDTO> GetUser(Guid id)
        {
            DAL.Domain.Entities.ApplicationUser? user = await Database.ApplicationUserRepository.GetEntityByIdAsync(id);
            if (user != null)
            {
                return _mapper.Map<ApplicationUserDTO>(user);
            }
            // Если null, выбрасываем исключение
            throw new ValidationException("Пользователь не найден", "");
        }
        public async Task<ApplicationUserDTO> GetUserByEmail(string email)
        {
            DAL.Domain.Entities.ApplicationUser user = await Database.ApplicationUserRepository.GetUserByEmail(email);
            if (user != null)
            {
                return _mapper.Map<DAL.Domain.Entities.ApplicationUser, ApplicationUserDTO>(user);
            }
            throw new ValidationException("Пользователь не найден", "");
        }
        public async Task<IEnumerable<ApplicationUserDTO>> GetAllUsers()
        {
            IEnumerable<DAL.Domain.Entities.ApplicationUser> users = await Database.ApplicationUserRepository.GetAllEntityesAsync();
            return _mapper.Map<IEnumerable<DAL.Domain.Entities.ApplicationUser>, IEnumerable<ApplicationUserDTO>>(users);
        }
        public async Task<bool> SignInResultSucceeded(string email, string password, bool rememberMe)
        {
            DAL.Domain.Entities.ApplicationUser applicationUser = await Database.ApplicationUserRepository.GetUserByEmail(email);
            if (applicationUser != null)
            {
                SignInResult result = await _signInManager.PasswordSignInAsync(applicationUser, password, rememberMe, false);
                return result.Succeeded;
            }
            // Если null, выбрасываем исключение "Пользователь не найден"
            throw new ValidationException("Пользователь не найден не найдена", "");
        }
        public void CreateUser(ApplicationUserDTO user)
        {
            DAL.Domain.Entities.ApplicationUser applicationUser = _mapper.Map<ApplicationUserDTO, DAL.Domain.Entities.ApplicationUser>(user);
            Database.ApplicationUserRepository.SaveEntity(applicationUser);
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
