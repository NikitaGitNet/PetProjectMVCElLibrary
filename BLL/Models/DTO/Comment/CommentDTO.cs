using BLL.Interfaces.DTO;
using DAL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.DTO.Comment
{
    public class CommentDTO : ICommentDTO
    {
        public Guid Id { get; set; }
        public string? CommentText { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? UserEmail { get; set; }
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        public Guid BookId { get; set; }
    }
}
