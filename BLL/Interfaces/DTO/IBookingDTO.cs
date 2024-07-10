using DAL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.DTO
{
    public interface IBookingDTO
    {
        Guid Id { get; set; }
        DateTime CreateOn { get; set; }
        DateTime FinishedOn { get; set; }
        bool IssueBooking { get; set; }
        string? UserEmail { get; set; }
        string? UserId { get; set; }
        Guid BookId { get; set; }
        string? BooksTitle { get; set; }
        string? ReceiptCode { get; set; }
    }
}
