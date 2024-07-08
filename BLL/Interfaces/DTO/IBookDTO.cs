using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.DTO
{
    public interface IBookDTO
    {
        Guid Id { get; set; }
        string? Title { get; set; }
        string? SubTitle { get; set; }
        string? AuthorName { get; set; }
        string? GenreName { get; set; }
        string? TitleImagePath { get; set; }
        string? CurentUserId { get; set; }
        string? Text { get; set; }
        bool IsBooking { get; set; }
        string? CommentText { get; set; }
        DateTime DateAdded { get; set; }
    }
}
