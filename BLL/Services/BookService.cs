using AutoMapper;
using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Interfaces.DTO;
using BLL.Models.DTO.Book;
using DAL.Domain;
using DAL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    /// <summary>
    /// Сервис обслуживаюший BookDTO, содержит бизнесс логику для работы с ним
    /// </summary>
    public class BookService : IBookService
    {
        private readonly UnitOfWorkRepository Database;
        private readonly IMapper mapper;
        public BookService(AppDbContext context)
        {
            Database = new UnitOfWorkRepository(context);
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookDTO>()).CreateMapper();
        }
        /// <summary>
        /// Обращаемся к BookRepository, получаем все книги, конвертируем в BookDTO
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BookDTO>> GetAllBooks()
        {
            // Получаем из базы коллекцию
            IEnumerable<Book> books = await Database.BookRepository.GetAllEntityesAsync();
            // Если книг нет, генерим исключение, что книг нету
            if (!books.Any())
            {
                throw new ValidationException("Книги не найдены", "");
            }
            // Конвертируем в DTO, возвращаем коллекцию DTO
            return mapper.Map<IEnumerable<Book>, List<BookDTO>>(books);
        }
        /// <summary>
        /// Обращаемся к BookRepository, получаем книгу по Id, конвертируем в BookDTO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task<BookDTO> GetBook(Guid id)
        {
            // Получаем из базы книгу по Id
            Book? book = await Database.BookRepository.GetEntityByIdAsync(id);
            if (book != null)
            {
                // Если не null, возвращаем DTO
                return mapper.Map<Book, BookDTO>(book);
            }
            // Если null, выбрасываем исключение "Книга не найдена"
            throw new ValidationException("Книга не найдена", "");
        }
        /// <summary>
        /// Добавляем книгу в базу
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task CreateBook(BookDTO bookDTO)
        {
            Book? book = await Database.BookRepository.GetEntityByIdAsync(bookDTO.Id);
            if (book != null)
            {
                throw new ValidationException("Книга уже существует", "");
            }
            book = mapper.Map<IBookDTO, Book>(bookDTO);
            if (book != null)
            {
                await Database.BookRepository.SaveEntityAsync(book);
            }
            else
            {
                throw new ValidationException("Не удалось сохранить книгу", "");
            }
        }
        /// <summary>
        /// Получение книг по жанру и мапинг их в ДТО
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task<IEnumerable<BookDTO>> GetBookByGenre(Guid genreId)
        {
            IEnumerable<Book> books = await Database.BookRepository.GetEntityesByGenreAsync(genreId);
            // Если книг нет, генерим исключение, что книг нету
            if (!books.Any())
            {
                throw new ValidationException("Книги не найдены", "");
            }
            // Конвертируем в DTO, возвращаем коллекцию DTO
            return mapper.Map<IEnumerable<Book>, List<BookDTO>>(books);
        }
        /// <summary>
        /// Получение  книг по автору и маппинг их в ДТО
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task<IEnumerable<BookDTO>> GetBookByAuthor(Guid authorId)
        {
            IEnumerable<Book> books = await Database.BookRepository.GetEntityesByAuthorAsync(authorId);
            // Если книг нет, генерим исключение, что книг нету
            if (!books.Any())
            {
                throw new ValidationException("Книги не найдены", "");
            }
            // Конвертируем в DTO, возвращаем коллекцию DTO
            return mapper.Map<IEnumerable<Book>, List<BookDTO>>(books);
        }
        /// <summary>
        /// Удаление книги из БД, на основании ИД
        /// </summary>
        /// <param name="bookId"></param>
        public void DeleteBook(Guid bookId)
        {
            Database.BookRepository.DeleteEntity(bookId);
        }
        /// <summary>
        /// Маппинг BookDTO в ентити, удаление ентити из БД, на основании массива ентити
        /// </summary>
        /// <param name="entityes"></param>
        public void DeleteRangeBooks(IEnumerable<BookDTO> entityes)
        {
            IEnumerable<Book> books = mapper.Map<IEnumerable<BookDTO>, IEnumerable<Book>>(entityes);
            Database.BookRepository.DeleteRangeEntityes(books);
        }
    }
}
