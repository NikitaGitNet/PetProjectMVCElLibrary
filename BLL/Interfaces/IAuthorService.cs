using BLL.Models.DTO.Author;

namespace BLL.Interfaces
{
    /// <summary>
    /// Интерфейс бизнес-логики взаимодействия с ДТО автора
    /// </summary>
    public interface IAuthorService
    {
        /// <summary>
        /// Добавление автора
        /// </summary>
        /// <param name="book"></param>
        bool CreateAuthor(AuthorDTO book);
        /// <summary>
        /// Jбновление автора
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        bool UpdateAuthor(AuthorDTO book);
        /// <summary>
        /// Получение ДТО автора по ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AuthorDTO?> GetAuthor(Guid id);
        /// <summary>
        /// Получение автора по названию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<AuthorDTO?> GetAuthorByName(string name);
        /// <summary>
        /// Получение ДТО всех авторов
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AuthorDTO>> GetAllAuthors();
        /// <summary>
        /// Удаление автора
        /// </summary>
        /// <param name="authorId"></param>
        void DeleteAuthor(Guid authorId);
    }
}
