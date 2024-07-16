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
        private readonly UserManager<DAL.Domain.Entities.ApplicationUser> _userManager;
        public ApplicationUserService(AppDbContext context, IMapper mapper, SignInManager<DAL.Domain.Entities.ApplicationUser> signInManager, UserManager<DAL.Domain.Entities.ApplicationUser> userManager)
        {
            Database = new UnitOfWorkRepository(context);
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<ApplicationUserDTO?> GetUser(Guid id)
        {
            DAL.Domain.Entities.ApplicationUser? user = await Database.ApplicationUserRepository.GetEntityByIdAsync(id);
            return _mapper.Map<ApplicationUserDTO?>(user);
        }
        public async Task<ApplicationUserDTO?> GetUserByEmail(string email)
        {
            DAL.Domain.Entities.ApplicationUser? user = await Database.ApplicationUserRepository.GetUserByEmail(email);
            return _mapper.Map<ApplicationUserDTO?>(user);
        }
        public async Task<IEnumerable<ApplicationUserDTO>> GetAllUsers()
        {
            IEnumerable<DAL.Domain.Entities.ApplicationUser> users = await Database.ApplicationUserRepository.GetAllEntityesAsync();
            return _mapper.Map<IEnumerable<DAL.Domain.Entities.ApplicationUser>, IEnumerable<ApplicationUserDTO>>(users);
        }
        public async Task<bool> SignInResultSucceeded(string email, string password, bool rememberMe)
        {
            DAL.Domain.Entities.ApplicationUser? applicationUser = await Database.ApplicationUserRepository.GetUserByEmail(email);
            if (applicationUser != null)
            {
                SignInResult result = await _signInManager.PasswordSignInAsync(applicationUser, password, rememberMe, false);
                return result.Succeeded;
            }
            return false;
        }
        public async Task<bool> ChangePassword(Guid userId, string password)
        {
            bool result = false;
            DAL.Domain.Entities.ApplicationUser? applicationUser = await Database.ApplicationUserRepository.GetEntityByIdAsync(userId);
            if (applicationUser != null)
            {
                applicationUser.PasswordHash = _userManager.PasswordHasher.HashPassword(applicationUser, password);
                var a = await _userManager.UpdateAsync(applicationUser);
                result = a.Succeeded;
                return result;
            }
            return result;
        }
        public void CreateUser(ApplicationUserDTO user)
        {
            DAL.Domain.Entities.ApplicationUser applicationUser = _mapper.Map<ApplicationUserDTO, DAL.Domain.Entities.ApplicationUser>(user);
            Database.ApplicationUserRepository.SaveEntity(applicationUser);
        }
        public void DeleteUser(Guid userId)
        {
            Database.ApplicationUserRepository.DeleteEntity(userId);
        }
        public void DeleteRangeUsers(IEnumerable<ApplicationUserDTO> users)
        {
            throw new NotImplementedException();
        }






    }
}
