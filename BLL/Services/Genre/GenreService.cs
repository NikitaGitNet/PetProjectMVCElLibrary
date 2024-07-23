using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.Genre;
using DAL.Domain;

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
            // Достаем из БД ентити
            DAL.Domain.Entities.Genre? genre = await Database.GenreRepository.GetEntityByIdAsync(id);
            // Мапим, возвращаем DTO
            return _mapper.Map<GenreDTO?>(genre);
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
        public bool CreateGenre(GenreDTO genreDTO)
        {
            DAL.Domain.Entities.Genre genre = _mapper.Map<DAL.Domain.Entities.Genre>(genreDTO);
            return Database.GenreRepository.SaveEntity(genre);
        }
        /// <summary>
        /// Маппинг ДТО в ентити, обновление ентити в БД
        /// </summary>
        /// <param name="genreDTO"></param>
        public bool UpdateGenre(GenreDTO genreDTO)
        {
            DAL.Domain.Entities.Genre genre = _mapper.Map<DAL.Domain.Entities.Genre>(genreDTO);
            return Database.GenreRepository.UpdateEntity(genre);
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
