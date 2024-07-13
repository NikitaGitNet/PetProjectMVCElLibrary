using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.TextField;
using DAL.Domain;

namespace BLL.Services.TextField
{
    public class TextFieldService : ITextFieldService
    {
        private readonly UnitOfWorkRepository Database;
        private readonly IMapper _mapper;
        public TextFieldService(AppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            Database = new UnitOfWorkRepository(context);
        }
        public async Task<IEnumerable<TextFieldDTO>> GetAllTextFields()
        {
            IEnumerable<DAL.Domain.Entities.TextField> textFields = await Database.TextFieldRepository.GetAllEntityesAsync();
            return _mapper.Map<IEnumerable<TextFieldDTO>>(textFields);
        }
        public async Task<TextFieldDTO?> GetTextField(Guid id)
        {
            DAL.Domain.Entities.TextField? textField = await Database.TextFieldRepository.GetEntityByIdAsync(id);
            return _mapper.Map<TextFieldDTO?>(textField);
        }
        public async Task<TextFieldDTO?> GetTextFieldByCodeWord(string codeWord)
        {
            DAL.Domain.Entities.TextField? textField = await Database.TextFieldRepository.GetTextFieldByCodeWord(codeWord);
            return _mapper.Map<TextFieldDTO?>(textField);
        }
        public void SaveTextField(TextFieldDTO textFieldDTO)
        {
            DAL.Domain.Entities.TextField textField = _mapper.Map<DAL.Domain.Entities.TextField>(textFieldDTO);
            Database.TextFieldRepository.SaveEntity(textField);
        }
    }
}
