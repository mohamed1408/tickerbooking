using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketBooking.Models;
using System.Security.Cryptography;
using System.Text;

namespace TicketBooking.Controllers
{
    public class AuthController : Controller
    {
        private BookingDBContext db;
        private readonly byte[] ConstantSalt = Encoding.UTF8.GetBytes("YourConstantSaltValue"); // Replace with your constant salt

        private const int KeySize = 32;  // 256-bit key size
        private const int Iterations = 10000; // Number of iterations for PBKDF2
        public AuthController(BookingDBContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            return Redirect("/Home/Search");
        }
        // GET: AuthController
        public ActionResult Login()
        {
            return View();
        }

        public IActionResult SignIn([FromForm]User user)
        {
            User _user = db.Users.Where(x => x.Email == user.Email && x.Password == HashPassword(user.Password)).FirstOrDefault();
            if(user == null)
            {
                return Ok(new { success = false });
            }
            // Assumption based on user identity exists auth db
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.Name),
                new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString())
                // Add additional claims as needed
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                // Set additional authentication properties if needed
            };

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Redirect("/Home/Search");
        }

        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { success = true });
        }
        public ActionResult Register()
        {
            return View();
        }

        public IActionResult SignUp([FromForm]User user)
        {
            user.Password = HashPassword(user.Password);
            db.Users.Add(user);
            db.SaveChanges();
            return View("Login");
        }
        public string HashPassword(string password)
        {
            // Hash the password with the constant salt using PBKDF2
            using (var deriveBytes = new Rfc2898DeriveBytes(password, ConstantSalt, Iterations))
            {
                byte[] hashBytes = deriveBytes.GetBytes(KeySize);

                // Convert to a base64 string for storage
                return Convert.ToBase64String(hashBytes);
            }
        }
        // GET: AuthController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuthController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuthController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuthController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
