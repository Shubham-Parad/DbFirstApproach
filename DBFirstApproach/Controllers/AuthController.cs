using DBFirstApproach.Data;
using DBFirstApproach.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DBFirstApproach.Controllers
{
    public class AuthController : Controller
    {
        private readonly EcommContext db;
        public AuthController(EcommContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public static string EncryptedPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] pass = ASCIIEncoding.ASCII.GetBytes(password);
                string encrpass = Convert.ToBase64String(pass);
                return encrpass;
            }
        }

        [HttpPost]
        public IActionResult SignUp(User u)
        {
            u.Password = EncryptedPassword(u.Password);
            u.Role = "User";
            db.Users.Add(u);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
