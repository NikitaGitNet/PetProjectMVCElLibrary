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
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
