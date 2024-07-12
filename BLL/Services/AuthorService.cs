using AutoMapper;
using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Models.DTO.Author;
using BLL.Models.DTO.Genre;
using DAL.Domain;
using DAL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
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
            IEnumerable<Author> authors = await Database.AuthorRepository.GetAllEntityesAsync();
            return _mapper.Map<IEnumerable<AuthorDTO>>(authors);
        }
        /// <summary>
        /// Получаем ентити конкретного автора из БД, маппим в ДТО
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task<AuthorDTO> GetAuthor(Guid id)
        {
            Author? author = await Database.AuthorRepository.GetEntityByIdAsync(id);
            if (author != null)
            {
                // Если не null, возвращаем DTO
                return _mapper.Map<AuthorDTO>(author);
            }
            // Если null, выбрасываем исключение "Книга не найдена"
            throw new ValidationException("Автор не найден", "");
        }
        public async Task<AuthorDTO?> GetAuthorByName(string name)
        {
            Author? author = await Database.AuthorRepository.GetEntityByNameAsync(name);
            return _mapper.Map<AuthorDTO>(author);
        }
        /// <summary>
        /// Маппим ДТО в энтити, сохраняем в БД
        /// </summary>
        /// <param name="genreDTO"></param>
        public void CreateAuthor(AuthorDTO genreDTO)
        {
            Author author = _mapper.Map<Author>(genreDTO);
            Database.AuthorRepository.SaveEntity(author);
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
