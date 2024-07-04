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

namespace BLL.Services
{
    /// <summary>
    /// Реализация паттерна UnitOfWork
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        // Контекст
        private readonly AppDbContext _context;
        // Перечень репозиториев
        public readonly IRepository<Book> BookRepository;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            BookRepository = new BookRepository(_context);
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
