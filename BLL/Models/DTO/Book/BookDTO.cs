using BLL.Interfaces.DTO;
using System.ComponentModel.DataAnnotations;


namespace BLL.Models.DTO.Book
{
    public class BookDTO : IBookDTO
    {
        public BookDTO()
        {
            Comments = new List<ICommentDTO>();
        }
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Заполните название книги")]
        [Display(Name = "Название книги")]
        public string? Title { get; set; }
        [Display(Name = "Краткое описание книги")]
        public string? SubTitle { get; set; }
        [Display(Name = "Полное описание книги")]
        public string? AuthorName { get; set; }
        [Display(Name = "Автор")]
        public string? GenreName { get; set; }
        [Display(Name = "Жанр")]
        public string? TitleImagePath { get; set; }
        public string? CurentUserId { get; set; }
        public string? Text { get; set; }
        public bool IsBooking { get; set; }
        public string? CommentText { get; set; }
        public DateTime DateAdded { get; set; }
        public IEnumerable<ICommentDTO> Comments { get; set; }
    }
}
