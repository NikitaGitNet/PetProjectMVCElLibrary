namespace DAL.Domain.Entities
{
    /// <summary>
    /// Энтити автора
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Наименование автора
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Связанные с ним книги, тип связи один ко многим
        /// </summary>
        public ICollection<Book> Books { get; set; } = null!;
    }
}
