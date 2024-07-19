using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Interfaces.Repository.TextField
{
    /// <summary>
    /// Интерфейс расширяющий логику взаимодействия с энтити текстового поля
    /// </summary>
    public interface ITextFieldRepository : IRepository<DAL.Domain.Entities.TextField>
    {
        /// <summary>
        /// Получение текстового поля по CodeWord
        /// </summary>
        /// <param name="codeWord"></param>
        /// <returns></returns>
        Task<DAL.Domain.Entities.TextField?> GetTextFieldByCodeWord(string codeWord);
    }
}
