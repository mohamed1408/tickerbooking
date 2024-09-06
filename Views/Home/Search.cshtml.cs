using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketBooking.Models;

namespace TicketBooking.Views.Home
{
    public class SearchModel : PageModel
    {
        public string? Location { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public List<Hotel> SearchResults { get; set; }

        public void OnGet()
        {
        }
    }
}
