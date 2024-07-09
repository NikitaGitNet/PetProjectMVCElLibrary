using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.DTO
{
    public interface ICommentDTO
    {
        Guid Id { get; set; }
        string? CommentText { get; set; }
        DateTime? CreateOn { get; set; }
        string? UserEmail { get; set; }
        string? UserName { get; set; }
        string? UserId { get; set; }
        Guid BookId { get; set; }
    }
}
