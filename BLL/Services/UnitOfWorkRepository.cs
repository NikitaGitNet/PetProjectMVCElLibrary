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

namespace BLL.Services
{
    /// <summary>
    /// Реализация паттерна UnitOfWork
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
        public UnitOfWorkRepository(AppDbContext context)
        {
            _context = context;
            BookRepository = new BookRepository(_context);
            CommentRepository = new CommentRepository(_context);
            ApplicationUserRepository = new ApplicationUserRepository(_context);
            BookingRepository = new BookingRepository(_context);
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
