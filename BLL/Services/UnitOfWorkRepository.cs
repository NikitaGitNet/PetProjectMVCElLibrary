﻿using DAL.Domain;
using DAL.Domain.Interfaces.Repository.Author;
using DAL.Domain.Interfaces.Repository.Book;
using DAL.Domain.Interfaces.Repository.Booking;
using DAL.Domain.Interfaces.Repository.Comment;
using DAL.Domain.Interfaces.Repository.Genre;
using DAL.Domain.Interfaces.Repository.TextField;
using DAL.Domain.Interfaces.Repository.User;
using DAL.Domain.Repository;
using Microsoft.AspNetCore.Identity;
using BLL.Models.DTO.ApplicationUser;

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
        public UnitOfWorkRepository(AppDbContext context)
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
