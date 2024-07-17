using AutoMapper;
using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Models.DTO.ApplicationUser;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services.ApplicationUser
{
    /// <summary>
    /// Класс содержащий бизнес-логику для взаимодействия с ДТО пользователя
    /// </summary>
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
        /// <summary>
        /// Получаем ДТО пользователя из БД на основании ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApplicationUserDTO?> GetUser(Guid id)
        {
            DAL.Domain.Entities.ApplicationUser? user = await Database.ApplicationUserRepository.GetEntityByIdAsync(id);
            return _mapper.Map<ApplicationUserDTO?>(user);
        }
        /// <summary>
        /// Получение ДТО пользователя из БД на основании email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<ApplicationUserDTO?> GetUserByEmail(string email)
        {
            DAL.Domain.Entities.ApplicationUser? user = await Database.ApplicationUserRepository.GetUserByEmail(email);
            return _mapper.Map<ApplicationUserDTO?>(user);
        }
        /// <summary>
        /// Получение коллекции ДТО всех пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ApplicationUserDTO>> GetAllUsers()
        {
            IEnumerable<DAL.Domain.Entities.ApplicationUser> users = await Database.ApplicationUserRepository.GetAllEntityesAsync();
            return _mapper.Map<IEnumerable<DAL.Domain.Entities.ApplicationUser>, IEnumerable<ApplicationUserDTO>>(users);
        }
        /// <summary>
        /// Метод проверки соответсвует ли текущий пользователь заявленной роли
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<bool> IsUserRoleConfirm(Guid userId, string roleName)
        {
            return await Database.ApplicationUserRepository.IsUserRoleConfirm(userId, roleName);
        }
        /// <summary>
        /// Метод авторизации пользователя в системе
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Метод смены пароля пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Метод создания/обновления пользователя
        /// </summary>
        /// <param name="user"></param>
        public void SaveUser(ApplicationUserDTO user)
        {
            DAL.Domain.Entities.ApplicationUser applicationUser = _mapper.Map<ApplicationUserDTO, DAL.Domain.Entities.ApplicationUser>(user);
            Database.ApplicationUserRepository.SaveEntity(applicationUser);
        }
        /// <summary>
        /// Метод удаления пользователя на основании ИД
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteUser(Guid userId)
        {
            Database.ApplicationUserRepository.DeleteEntity(userId);
        }
        /// <summary>
        /// Метод массового удаления пользователей на основании коллекции ДТО пользователей
        /// </summary>
        /// <param name="users"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void DeleteRangeUsers(IEnumerable<ApplicationUserDTO> users)
        {
            throw new NotImplementedException();
        }

        
    }
}
