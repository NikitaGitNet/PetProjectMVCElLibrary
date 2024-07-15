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
    public class TextFieldRepository : ITextFieldRepository
    {
        private readonly AppDbContext _context;
        public TextFieldRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TextField>> GetAllEntityesAsync()
        {
            return await _context.TextFields.ToListAsync();
        }
        public async Task<TextField?> GetEntityByIdAsync(Guid id)
        {
            return await _context.TextFields.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<TextField?> GetTextFieldByCodeWord(string codeWord)
        {
            return await _context.TextFields.FirstOrDefaultAsync(x => x.CodeWord == codeWord);
        }
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
        public void DeleteEntity(Guid id)
        {
            throw new NotImplementedException();
        }
        public void DeleteRangeEntityes(IEnumerable<TextField> entityes)
        {
            throw new NotImplementedException();
        }
    }
}
