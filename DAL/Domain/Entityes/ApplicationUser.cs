using Microsoft.AspNetCore.Identity;

namespace DAL.Domain.Entities
{
    /// <summary>
    /// Энтити пользователя, наследуемся от базовой модели IdentityUser, добавляем кастомные поля
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Дата регистрации пользователя
        /// </summary>
        public DateTime CreateOn { get; set; }
        /// <summary>
        /// Комменты пользователя, тип связи один ко многим
        /// </summary>
        public IEnumerable<Comment>? Comments { get; set; }
        /// <summary>
        /// Брони пользователя, тип связи один ко многим
        /// </summary>
        public IEnumerable<Booking>? Bookings { get; set; }
    }
}
