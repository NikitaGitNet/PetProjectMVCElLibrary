using DAL.Domain.Repository;
using DAL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Domain.Interfaces.Repository;
using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository.Book;
using DAL.Domain.Interfaces.Repository.Comment;
using DAL.Domain.Interfaces.Repository.User;
using DAL.Domain.Interfaces.Repository.Booking;
using DAL.Domain.Interfaces.Repository.Genre;
using DAL.Domain.Interfaces.Repository.Author;
using DAL.Domain.Interfaces.Repository.TextField;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    /// <summary>
    /// Реализация паттерна UnitOfWork, суем ссылки на репозитории в одно место, чтоб удобнее было с ними работать
    /// </summary>
    public class UnitOfWorkRepository : IDisposable
    {
        // Контекст
        private readonly AppDbContext _context;
        // Перечень репозиториев
        public readonly IBookRepository BookRepository;
        public readonly IBookingRepository BookingRepository;
        public readonly ICommentRepository CommentRepository;
        public readonly IApplicationUserRepository ApplicationUserRepository;
        public readonly IGenreRepository GenreRepository;
        public readonly IAuthorRepository AuthorRepository;
        public readonly ITextFieldRepository TextFieldRepository;
        public UnitOfWorkRepository(AppDbContext context, SignInManager<ApplicationUser> _signInManager, )
        {
            _context = context;
            BookRepository = new BookRepository(_context);
            CommentRepository = new CommentRepository(_context);
            ApplicationUserRepository = new ApplicationUserRepository(_context);
            BookingRepository = new BookingRepository(_context);
            GenreRepository = new GenreRepository(_context);
            AuthorRepository = new AuthorRepository(_context);
            TextFieldRepository = new TextFieldRepository(_context);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
