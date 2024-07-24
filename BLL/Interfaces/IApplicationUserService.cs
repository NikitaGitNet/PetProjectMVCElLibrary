using BLL.Models.DTO.ApplicationUser;

namespace BLL.Interfaces
{
    /// <summary>
    /// Интерфейс бизнес-логики взаимодействия с ДТО пользователя
    /// </summary>
    public interface IApplicationUserService
    {
        /// <summary>
        /// Получаем ДТО пользователя из БД на основании ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApplicationUserDTO?> GetUser(Guid id);
        /// <summary>
        /// Получение ДТО пользователя из БД на основании email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<ApplicationUserDTO?> GetUserByEmail(string email);
        /// <summary>
        /// Получение коллекции ДТО всех пользователей
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUserDTO>> GetAllUsers();
        /// <summary>
        /// Метод авторизации пользователя в системе
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        Task<bool> SignInResultSucceeded(string email, string password, bool rememberMe);
        /// <summary>
        /// Авторизация пользователя в системе
        /// </summary>
        /// <param name="applicationUserDTO"></param>
        /// <returns></returns>
        Task SignIn(ApplicationUserDTO applicationUserDTO);
        /// <summary>
        /// Метод проверки соответсвует ли текущий пользователь заявленной роли
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<bool> IsUserRoleConfirm(Guid userId, string roleName);
        /// <summary>
        /// Метод создания/обновления пользователя
        /// </summary>
        /// <param name="user"></param>
        Task<bool> CreateUser(ApplicationUserDTO user);
        /// <summary>
        /// Метод смены пароля пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> ChangePassword(Guid userId, string password);
        /// <summary>
        /// Метод удаления пользователя на основании ИД
        /// </summary>
        /// <param name="userId"></param>
        void DeleteUser(Guid userId);
        /// <summary>
        /// Метод массового удаления пользователей на основании коллекции ДТО пользователей
        /// </summary>
        /// <param name="users"></param>
        void DeleteRangeUsers(IEnumerable<ApplicationUserDTO> users);
    }
}
