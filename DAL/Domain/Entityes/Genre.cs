using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Entities
{
    /// <summary>
    /// Энтити жанра
    /// </summary>
    public class Genre
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Название жанра
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Связанные книги, тип связи один ко многим
        /// </summary>
        public ICollection<Book>? Books { get; set; }
    }
}
