using BLL.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.DTO.TextField
{
    public class TextFieldDTO : ITextFieldDTO
    {
        public Guid Id { get; set; }
        public string? CodeWord { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
    }
}
