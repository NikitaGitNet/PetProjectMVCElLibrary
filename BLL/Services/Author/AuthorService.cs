using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.Author;
using DAL.Domain;

namespace BLL.Services.Author
{
    public class AuthorService : IAuthorService
    {
        private readonly UnitOfWorkRepository Database;
        private readonly IMapper _mapper;
        public AuthorService(AppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            Database = new UnitOfWorkRepository(context);
        }
        /// <summary>
        /// Получаем всех ентити авторов из бд, маппим в ДТО
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<AuthorDTO>> GetAllAuthors()
        {
            IEnumerable<DAL.Domain.Entities.Author> authors = await Database.AuthorRepository.GetAllEntityesAsync();
            return _mapper.Map<IEnumerable<AuthorDTO>>(authors);
        }
        /// <summary>
        /// Получаем ентити конкретного автора из БД, маппим в ДТО
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task<AuthorDTO?> GetAuthor(Guid id)
        {
            // Достаем из БД ентити
            DAL.Domain.Entities.Author? author = await Database.AuthorRepository.GetEntityByIdAsync(id);
            // Мапим, возвращаем DTO
            return _mapper.Map<AuthorDTO>(author);
        }
        /// <summary>
        /// Получаем автора по наименованию, мапим в ДТО возвращаем
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<AuthorDTO?> GetAuthorByName(string name)
        {
            DAL.Domain.Entities.Author? author = await Database.AuthorRepository.GetEntityByNameAsync(name);
            return _mapper.Map<AuthorDTO>(author);
        }
        /// <summary>
        /// Маппим ДТО в энтити, сохраняем в БД
        /// </summary>
        /// <param name="genreDTO"></param>
        public bool CreateAuthor(AuthorDTO genreDTO)
        {
            DAL.Domain.Entities.Author author = _mapper.Map<DAL.Domain.Entities.Author>(genreDTO);
            return Database.AuthorRepository.SaveEntity(author);
        }
        /// <summary>
        /// Маппим ДТО в энтити, обновляем в БД
        /// </summary>
        /// <param name="genreDTO"></param>
        public bool UpdateAuthor(AuthorDTO genreDTO)
        {
            DAL.Domain.Entities.Author author = _mapper.Map<DAL.Domain.Entities.Author>(genreDTO);
            return Database.AuthorRepository.UpdateEntity(author);
        }
        /// <summary>
        /// Удаляем ентити автора из БД на основании ID
        /// </summary>
        /// <param name="bookId"></param>
        public void DeleteAuthor(Guid bookId)
        {
            Database.AuthorRepository.DeleteEntity(bookId);
        }
    }
}
