using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Entities
{
    /// <summary>
    /// Энтити комментария
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Содержимое комментария
        /// </summary>
        public string? Text { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime? CreateOn { get; set; }
        /// <summary>
        /// Emai пользователя, который написал его
        /// </summary>
        public string? UserEmail { get; set; }
        /// <summary>
        /// Юзер нейм пользователя
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// Внешний ключ пользователя, тип связи один ко многим
        /// </summary>
        [ForeignKey("UserId")]
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; }
        /// <summary>
        /// Внешний ключ книги, тип связи один ко многим
        /// </summary>
        [ForeignKey("BookId")]
        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}
