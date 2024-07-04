using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DAL.Domain.Entities
{
    /// <summary>
    /// Энтити книги
    /// </summary>
    public class Book
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Заполните название книги")]
        [Display(Name = "Название книги")]
        public string? Title { get; set; }
        [Display(Name = "Краткое описание книги")]
        public string? SubTitle { get; set; }
        [Display(Name = "Полное описание книги")]
        public bool IsBooking { get; set; }
        public string? Text { get; set; }
        [Display(Name = "Автор книги, Заполнить в формате - Фамилия Имя Отчество")]
        public string? AuthorName { get; set; }
        public Guid AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Author? Author { get; set; }
        [Display(Name = "Жанр книги")]
        public Guid GenreId { get; set; }
        [ForeignKey("GenreId")]
        public Genre? Genre { get; set; }
        public string? GenreName { get; set; }
        public string? TitleImagePath { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }
}
