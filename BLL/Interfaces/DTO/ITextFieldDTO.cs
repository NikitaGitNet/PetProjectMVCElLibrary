namespace BLL.Interfaces.DTO
{
    /// <summary>
    /// Интерфейс ДТО текстового поля
    /// </summary>
    public interface ITextFieldDTO
    {
        /// <summary>
        /// ИД текстового поля
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// CodeWord текстового поля
        /// </summary>
        string? CodeWord { get; set; }
        /// <summary>
        /// Название текстового поля
        /// </summary>
        string? Title { get; set; }
        /// <summary>
        /// Содержимое текстового поля
        /// </summary>
        string? Text { get; set; }
    }
}
