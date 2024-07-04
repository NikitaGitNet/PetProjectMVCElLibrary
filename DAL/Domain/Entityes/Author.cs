using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Entities
{
    /// <summary>
    /// Энтити автора
    /// </summary>
    public class Author
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        ICollection<Book>? Books { get; set; }
    }
}
