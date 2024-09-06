namespace TicketBooking.Models
{
    public class ProfileModel
    {
        public User user { get; set; }
        public List<Booking> bookings { get; set; }
    }
}
