using BLL.Models.DTO.Book;
using BLL.Models.DTO.TextField;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    /// <summary>
    /// Интерфейс бизнес-логики взаимодействия с ДТО текстового поля
    /// </summary>
    public interface ITextFieldService
    {
        /// <summary>
        /// Сохранение/обновление текстового поля
        /// </summary>
        /// <param name="textFieldDTO"></param>
        void SaveTextField(TextFieldDTO textFieldDTO);
        /// <summary>
        /// Получение ДТО текстового поля по ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TextFieldDTO?> GetTextField(Guid id);
        /// <summary>
        /// Получение ДТО всех текстовых полей
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TextFieldDTO>> GetAllTextFields();
        /// <summary>
        /// Получение ДТО текстового поля по CodeWord
        /// </summary>
        /// <param name="codeWord"></param>
        /// <returns></returns>
        Task<TextFieldDTO?> GetTextFieldByCodeWord(string codeWord);
    }
}

