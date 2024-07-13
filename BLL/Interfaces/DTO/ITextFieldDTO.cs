using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.DTO
{
    public interface ITextFieldDTO
    {
        Guid Id { get; set; }
        string? CodeWord { get; set; }
        string? SubTitle { get; set; }
        string? TitleImagePath { get; set; }
        string? Title { get; set; }
        string? Text { get; set; }
    }
}
