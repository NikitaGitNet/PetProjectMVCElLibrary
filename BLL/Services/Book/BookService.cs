﻿using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.Book;
using DAL.Domain;

namespace BLL.Services.Book
{
    /// <summary>
    /// Сервис обслуживаюший BookDTO, содержит бизнесс логику для работы с ним
    /// </summary>
    public class BookService : IBookService
    {
        private readonly UnitOfWorkRepository Database;
        private readonly IMapper _mapper;
        public BookService(AppDbContext context, IMapper mapper)
        {
            Database = new UnitOfWorkRepository(context);
            _mapper = mapper;
        }
        /// <summary>
        /// Обращаемся к BookRepository, получаем все книги, конвертируем в BookDTO
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BookDTO>> GetAllBooks()
        {
            // Получаем из базы коллекцию
            IEnumerable<DAL.Domain.Entities.Book> books = await Database.BookRepository.GetAllEntityesAsync();
            return _mapper.Map<IEnumerable<BookDTO>>(books);
        }
        /// <summary>
        /// Обращаемся к BookRepository, получаем книгу по Id, конвертируем в BookDTO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task<BookDTO?> GetBook(Guid id)
        {
            // Получаем из базы книгу по Id
            DAL.Domain.Entities.Book? book = await Database.BookRepository.GetEntityByIdAsync(id);
            return _mapper.Map<BookDTO?>(book);
        }
        /// <summary>
        /// Добавляем книгу в базу
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public bool CreateBook(BookDTO bookDTO)
        {
            DAL.Domain.Entities.Book book = _mapper.Map<DAL.Domain.Entities.Book>(bookDTO);
            return Database.BookRepository.SaveEntity(book);
        }
        /// <summary>
        /// Обновляем книгу в базе
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        public bool UpdateBook(BookDTO bookDTO)
        {
            DAL.Domain.Entities.Book book = _mapper.Map<DAL.Domain.Entities.Book>(bookDTO);
            return Database.BookRepository.UpdateEntity(book);
        }
        /// <summary>
        /// Маппим массив ДТО в ентити, массово обновляем книги в БД
        /// </summary>
        /// <param name="bookDTOs"></param>
        public void UpdateBooksRange(IEnumerable<BookDTO> bookDTOs)
        {
            IEnumerable<DAL.Domain.Entities.Book> books = _mapper.Map<IEnumerable<DAL.Domain.Entities.Book>>(bookDTOs);
            Database.BookRepository.UpdateEntityRange(books);
        }
        /// <summary>
        /// Получение книг по жанру и мапинг их в ДТО
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task<IEnumerable<BookDTO>> GetBookByGenre(Guid genreId)
        {
            IEnumerable<DAL.Domain.Entities.Book> books = await Database.BookRepository.GetEntityesByGenreAsync(genreId);
            return _mapper.Map<IEnumerable<BookDTO>>(books);

        }
        /// <summary>
        /// Получение  книг по автору и маппинг их в ДТО
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task<IEnumerable<BookDTO>> GetBookByAuthor(Guid authorId)
        {
            IEnumerable<DAL.Domain.Entities.Book> books = await Database.BookRepository.GetEntityesByAuthorAsync(authorId);
            return _mapper.Map<IEnumerable<BookDTO>>(books);

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
            IEnumerable<DAL.Domain.Entities.Book> books = _mapper.Map<IEnumerable<DAL.Domain.Entities.Book>>(entityes);
            Database.BookRepository.DeleteRangeEntityes(books);
        }
    }
}
