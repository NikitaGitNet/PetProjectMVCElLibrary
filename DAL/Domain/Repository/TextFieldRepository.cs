using DAL.Domain.Entities;
using DAL.Domain.Interfaces.Repository.TextField;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Repository
{
    /// <summary>
    /// Реализация репозитория для текстового поля
    /// Содержит логику взаимодействия с энтити текстового поля
    /// </summary>
    public class TextFieldRepository : ITextFieldRepository
    {
        private readonly AppDbContext _context;
        public TextFieldRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Получаем все текстовые поля из базы данных
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TextField>> GetAllEntityesAsync()
        {
            return await _context.TextFields.ToListAsync();
        }
        /// <summary>
        /// Получение энтити текстового поля по ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TextField?> GetEntityByIdAsync(Guid id)
        {
            return await _context.TextFields.FirstOrDefaultAsync(x => x.Id == id);
        }
        /// <summary>
        /// Получение текстового поля по CodeWord
        /// </summary>
        /// <param name="codeWord"></param>
        /// <returns></returns>
        public async Task<TextField?> GetTextFieldByCodeWord(string codeWord)
        {
            return await _context.TextFields.FirstOrDefaultAsync(x => x.CodeWord == codeWord);
        }
        /// <summary>
        /// Сохранение/обновление энтити текстового поля в БД
        /// </summary>
        /// <param name="entity"></param>
        public void SaveEntity(TextField entity)
        {
            TextField? textField = _context.TextFields.FirstOrDefault(x => x.Id == entity.Id);
            if (textField != null)
            {
                _context.TextFields.Update(entity);
            }
            else
            {
                _context.TextFields.Add(entity);
            }
            _context.SaveChanges();
        }
        /// <summary>
        /// Удаление текстового поля из бд
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void DeleteEntity(Guid id)
        {
            // Не реализованно за ненадобностью
            throw new NotImplementedException();
        }
        /// <summary>
        /// Массовое удаление текстовых полей из БД
        /// </summary>
        /// <param name="entityes"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void DeleteRangeEntityes(IEnumerable<TextField> entityes)
        {
            // Не реализованно за ненадобностью
            throw new NotImplementedException();
        }
    }
}
