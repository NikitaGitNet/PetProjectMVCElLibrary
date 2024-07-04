﻿using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Repository
{
    /// <summary>
    /// Методы взаимодействия с entity Booking
    /// </summary>
    public class BookingRepository : IRepository<Booking>
    {
        private readonly AppDbContext _context;
        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Получение всех имеющихся броней из БД
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Booking>> GetAllEntityesAsync()
        {
            return await _context.Bookings.ToListAsync();
        }
        /// <summary>
        /// Получение entity по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Booking?> GetEntityByIdAsync(Guid id)
        {
            return await _context.Bookings.FirstOrDefaultAsync(x => x.Id == id);
        }
        /// <summary>
        /// Получение броней из БД по массиву идентификаторов
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Booking>> GetEntityesByIdsAsync(IEnumerable<Guid> ids)
        {
            return await _context.Bookings.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
        /// <summary>
        /// Сохранение брони в БД
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task SaveEntityAsync(Booking entity)
        {
            await _context.Bookings.AddAsync(entity);
            _context.SaveChanges();
        }
        /// <summary>
        /// Сохрание коллекции броней в БД
        /// </summary>
        /// <param name="entityes"></param>
        /// <returns></returns>
        public async Task SaveRangeEntityesAsync(IEnumerable<Booking> entityes)
        {
            await _context.Bookings.AddRangeAsync(entityes);
            _context.SaveChanges();
        }
        /// <summary>
        /// Удаление брони по идентификатору
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEntity(Guid id)
        {
            _context.Bookings.Remove(new Booking() { Id = id });
            _context.SaveChanges();
        }
        /// <summary>
        /// Массовое удаление броней по массиву entityes
        /// </summary>
        /// <param name="entityes"></param>
        public void DeleteRangeEntityes(IEnumerable<Booking> entityes)
        {
            _context.Bookings.RemoveRange(entityes);
            _context.SaveChanges();
        }
    }
}
