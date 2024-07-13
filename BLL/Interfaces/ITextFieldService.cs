using BLL.Models.DTO.Book;
using BLL.Models.DTO.TextField;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ITextFieldService
    {
        void SaveTextField(TextFieldDTO textFieldDTO);
        Task<TextFieldDTO?> GetTextField(Guid id);
        Task<IEnumerable<TextFieldDTO>> GetAllTextFields();
        Task<TextFieldDTO?> GetTextFieldByCodeWord(string codeWord);
    }
}

