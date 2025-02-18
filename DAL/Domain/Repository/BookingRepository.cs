﻿using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository.Booking;
using Microsoft.EntityFrameworkCore;

namespace DAL.Domain.Repository
{
    /// <summary>
    /// Методы взаимодействия с entity Booking
    /// </summary>
    public class BookingRepository : IBookingRepository
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
        /// Получение броней из БД по ИД пользователя к которому, они привязаны
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Booking>> GetBookingByUserIdAsync(string id)
        { 
            return await _context.Bookings.Where(x => x.UserId == id).ToListAsync();
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
        /// Сохранение брони в БД
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SaveEntity(Booking entity)
        {
            bool result = false;
            try
            {
                Booking? booking = _context.Bookings.FirstOrDefault(x => x.Id == entity.Id);
                if (booking == null)
                {
                    _context.Bookings.Add(entity);
                    _context.SaveChanges();
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// Обновление брони в БД
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateEntity(Booking entity)
        {
            bool result = false;
            try
            {
                Booking? booking = _context.Bookings.FirstOrDefault(x => x.Id == entity.Id);
                if (booking != null)
                {
                    _context.Bookings.Update(entity);
                    _context.SaveChanges();
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
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
