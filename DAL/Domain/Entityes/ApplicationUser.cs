using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreateOn { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }
        public IEnumerable<Booking>? Bookings { get; set; }
    }
}
