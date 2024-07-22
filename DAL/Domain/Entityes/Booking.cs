using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Entities
{
    /// <summary>
    /// Энтити брони
    /// </summary>
    public class Booking
    {
        /// <summary>
        /// ИД брони, первичный ключ
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Дата создания брони
        /// </summary>
        public DateTime CreateOn { get; set; }
        /// <summary>
        /// Дата окончания брони
        /// </summary>
        public DateTime FinishedOn { get; set; }
        /// <summary>
        /// Флаг - выдана книга или нет
        /// </summary>
        public bool IssueBooking { get; set; }
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// ИД пользователя, внешний ключ, тип связи один ко многим
        /// </summary>
        [ForeignKey("UserId")]
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        /// <summary>
        /// ИД книги, внешний ключ, тип связи один ко многим
        /// </summary>
        [ForeignKey("Id")]
        public Guid BookId { get; set; }
        /// <summary>
        /// Название книги
        /// </summary>
        public string? BooksTitle { get; set; }
        public Book Book { get; set; } = null!;
        /// <summary>
        /// Уникальный код, согласно бизнес-модели по этому коду бользователь получаем книгу в библиотеки
        /// </summary>
        public string? ReceiptCode { get; set; }
    }
}
