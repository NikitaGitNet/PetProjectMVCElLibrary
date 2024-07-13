using AutoMapper;
using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Models.DTO.Genre;
using DAL.Domain;
using DAL.Domain.Entities;

namespace BLL.Services.Genre
{
    /// <summary>
    /// Сервис обслуживающий GenreDTO, содержит бизнес логику для работы с ним
    /// </summary>
    public class GenreService : IGenreService
    {
        private readonly UnitOfWorkRepository Database;
        private readonly IMapper _mapper;
        public GenreService(AppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            Database = new UnitOfWorkRepository(context);
        }
        /// <summary>
        /// Получение всех жанров из БД, маппинг их в ДТО
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<GenreDTO>> GetAllGenres()
        {
            IEnumerable<DAL.Domain.Entities.Genre> genres = await Database.GenreRepository.GetAllEntityesAsync();
            return _mapper.Map<IEnumerable<GenreDTO>>(genres);
        }
        /// <summary>
        /// Получение конкретного жанра из БД маппинг его в ДТО
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task<GenreDTO?> GetGenre(Guid id)
        {
            DAL.Domain.Entities.Genre? genre = await Database.GenreRepository.GetEntityByIdAsync(id);
            if (genre != null)
            {
                // Если не null, возвращаем DTO
                return _mapper.Map<GenreDTO>(genre);
            }
            // Если null, выбрасываем исключение "Книга не найдена"
            throw new ValidationException("Жанр не найден", "");
        }
        /// <summary>
        /// Получаем из БД жанр, маппим в ДТО, возвращаем
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<GenreDTO?> GetGenreByName(string name)
        {
            DAL.Domain.Entities.Genre? genre = await Database.GenreRepository.GetEntityByNameAsync(name);
            return _mapper.Map<GenreDTO>(genre);
        }
        /// <summary>
        /// Маппинг ДТО в ентити, сохранение ентити в БД
        /// </summary>
        /// <param name="genreDTO"></param>
        public void CreateGenre(GenreDTO genreDTO)
        {
            DAL.Domain.Entities.Genre genre = _mapper.Map<DAL.Domain.Entities.Genre>(genreDTO);
            Database.GenreRepository.SaveEntity(genre);
        }
        /// <summary>
        /// Удаление ентити на основании ИД
        /// </summary>
        /// <param name="bookId"></param>
        public void DeleteGenre(Guid bookId)
        {
            Database.GenreRepository.DeleteEntity(bookId);
        }
    }
}
