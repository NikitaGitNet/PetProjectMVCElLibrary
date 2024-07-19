using BLL.Models.DTO.Genre;

namespace BLL.Interfaces
{
    /// <summary>
    /// Интерфейс бизнес-логики взаимодействия с ДТО жанра
    /// </summary>
    public interface IGenreService
    {
        /// <summary>
        /// Сохранение/обновление жанра
        /// </summary>
        /// <param name="book"></param>
        void CreateGenre(GenreDTO book);
        /// <summary>
        /// Получение ДТО жанра по ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GenreDTO?> GetGenre(Guid id);
        /// <summary>
        /// Получение ДТО жанра по названию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<GenreDTO?> GetGenreByName(string name);
        /// <summary>
        /// Получение ДТО всех жанров
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<GenreDTO>> GetAllGenres();
        /// <summary>
        /// Удаление жанра
        /// </summary>
        /// <param name="genreId"></param>
        void DeleteGenre(Guid genreId);
    }
}
