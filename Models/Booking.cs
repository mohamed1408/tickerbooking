using System.ComponentModel.DataAnnotations;

namespace TicketBooking.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string TicketId { get; set; }
        public int HotelId { get; set; }
        public int UserId { get; set; }

        [DataType(DataType.Date)]
        public DateTime From { get; set; }

        [DataType(DataType.Date)]
        public DateTime To { get; set; }
    }
}
