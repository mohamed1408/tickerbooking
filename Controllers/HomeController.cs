using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TicketBooking.Models;
using TicketBooking.Views.Home;

namespace TicketBooking.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private BookingDBContext db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, BookingDBContext _db)
        {
            db = _db;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Search(string location, DateTime? from, DateTime? to)
        {
            var viewModel = new SearchModel();
            viewModel.Location = location;
            viewModel.From = from.HasValue ? ((DateTime)from).ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
            viewModel.To = to.HasValue ? ((DateTime)to).ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
            viewModel.SearchResults = new List<Hotel>();
            if (string.IsNullOrEmpty(location) || !from.HasValue || !to.HasValue)
            {
                // Return view with empty search results
                return View(viewModel);
            }
            viewModel.SearchResults = db.Hotels.ToList();
            return View(viewModel);
        }
        public IActionResult BookHotel(int id, DateTime from, DateTime to)
        {
            try
            {
                Booking booking = new Booking();
                booking.HotelId = id;
                booking.TicketId = Guid.NewGuid().ToString();
                booking.From = from;
                booking.To = to;
                booking.UserId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
                db.Bookings.Add(booking);
                db.SaveChanges();
                return Ok(new { success = true, redirecturl = Url.Action("Profile", "Home") });
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
