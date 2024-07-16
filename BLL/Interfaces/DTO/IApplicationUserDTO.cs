using BLL.Models.DTO.Booking;
using BLL.Models.DTO.Comment;
using DAL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.DTO
{
    internal interface IApplicationUserDTO
    {
        string? Id { get; set; }
        string? Email { get; set; }
        string? UserName { get; set; }
        ICollection<CommentDTO> Comments { get; set; }
        IEnumerable<BookingDTO> Bookings { get; set; }
    }
}
