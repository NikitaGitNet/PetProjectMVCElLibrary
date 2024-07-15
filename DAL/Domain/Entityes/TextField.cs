using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Entities
{
    public class TextField
    {
        public Guid Id { get; set; }
        [Required]
        public string? CodeWord { get; set; }
        [Display(Name = "Название страницы (заголовок)")]
        public string? Title { get; set; }
        [Display(Name = "Содержание страницы")]
        public string? Text { get; set; }
    }
}
