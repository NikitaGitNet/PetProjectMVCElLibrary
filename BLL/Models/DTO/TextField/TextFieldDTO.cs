using BLL.Interfaces.DTO;

namespace BLL.Models.DTO.TextField
{
    /// <summary>
    /// ДТО текстового поля
    /// </summary>
    public class TextFieldDTO : ITextFieldDTO
    {
        /// <summary>
        /// ИД текстового поля
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// CodeWord текстового поля
        /// </summary>
        public string? CodeWord { get; set; }
        /// <summary>
        /// Название текстового поля
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Содержимое текстового поля
        /// </summary>
        public string? Text { get; set; }
    }
}
