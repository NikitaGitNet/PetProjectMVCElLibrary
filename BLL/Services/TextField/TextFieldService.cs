using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.TextField;
using DAL.Domain;

namespace BLL.Services.TextField
{
    /// <summary>
    /// Бизнес-логика взаимодействия с ДТО текстового поля
    /// </summary>
    public class TextFieldService : ITextFieldService
    {
        private readonly UnitOfWorkRepository Database;
        private readonly IMapper _mapper;
        public TextFieldService(AppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            Database = new UnitOfWorkRepository(context);
        }
        /// <summary>
        /// Получаем коллекцию энтити из БД, мапим в коллекцию ДТО, возвращаем
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TextFieldDTO>> GetAllTextFields()
        {
            IEnumerable<DAL.Domain.Entities.TextField> textFields = await Database.TextFieldRepository.GetAllEntityesAsync();
            return _mapper.Map<IEnumerable<TextFieldDTO>>(textFields);
        }
        /// <summary>
        /// Получаем энтити по ИД, мапим в ДТО, возвращаем
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TextFieldDTO?> GetTextField(Guid id)
        {
            DAL.Domain.Entities.TextField? textField = await Database.TextFieldRepository.GetEntityByIdAsync(id);
            return _mapper.Map<TextFieldDTO?>(textField);
        }
        /// <summary>
        /// Получаем энтити по CodeWord, мапим в ДТО, возвращаем
        /// </summary>
        /// <param name="codeWord"></param>
        /// <returns></returns>
        public async Task<TextFieldDTO?> GetTextFieldByCodeWord(string codeWord)
        {
            DAL.Domain.Entities.TextField? textField = await Database.TextFieldRepository.GetTextFieldByCodeWord(codeWord);
            return _mapper.Map<TextFieldDTO?>(textField);
        }
        /// <summary>
        /// Сохранение/обновление текстового поля
        /// Мапим ДТО в энтити, сохраняем
        /// </summary>
        /// <param name="textFieldDTO"></param>
        public void SaveTextField(TextFieldDTO textFieldDTO)
        {
            DAL.Domain.Entities.TextField textField = _mapper.Map<DAL.Domain.Entities.TextField>(textFieldDTO);
            Database.TextFieldRepository.SaveEntity(textField);
        }
    }
}
