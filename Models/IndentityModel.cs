using Microsoft.EntityFrameworkCore;

namespace TicketBooking.Models
{
    public class BookingDBContext: DbContext
    {
        public BookingDBContext(DbContextOptions<BookingDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
