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
        /// ИД брони
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
        /// ИД пользователя
        /// </summary>
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        /// <summary>
        /// ИД книги
        /// </summary>
        [ForeignKey("Id")]
        public Guid BookId { get; set; }
        /// <summary>
        /// Название книги
        /// </summary>
        public string? BooksTitle { get; set; }
        public Book? Book { get; set; }
        public string? ReceiptCode { get; set; }
    }
}
