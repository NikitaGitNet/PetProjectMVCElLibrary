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
        public Booking()
        {
            User = new ApplicationUser();
            Book = new Book();
        }
        public Guid Id { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime FinishedOn { get; set; }
        public bool IssueBooking { get; set; }
        public string? UserEmail { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public Guid BookId { get; set; }
        [ForeignKey("Id")]
        public string? BooksTitle { get; set; }
        public Book Book { get; set; }
    }
}
