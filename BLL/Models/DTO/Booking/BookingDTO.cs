using BLL.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.DTO.Booking
{
    public class BookingDTO : IBookingDTO
    {
        public Guid Id { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime FinishedOn { get; set; }
        public bool IssueBooking { get; set; }
        public string? UserEmail { get; set; }
        public string? UserId { get; set; }
        public Guid BookId { get; set; }
        public string? BooksTitle { get; set; }
        public string? ReceiptCode { get; set; }
    }
}
