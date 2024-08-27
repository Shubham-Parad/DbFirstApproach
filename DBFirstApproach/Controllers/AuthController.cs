using DBFirstApproach.Data;
using DBFirstApproach.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        public static string DecryptedPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] pass = Convert.FromBase64String(password);
                string decrpass = ASCIIEncoding.ASCII.GetString(pass);
                return decrpass;
            }
        }

        [HttpPost]
        public IActionResult SignUp(User u)
        {
            u.Password = EncryptedPassword(u.Password);
            u.Role = "User";
            db.Users.Add(u);
            db.SaveChanges();
            return RedirectToAction("SignIn");
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(LoginModel log)
        {
            var data = db.Users.Where(x => x.Username.Equals(log.Username)).SingleOrDefault();
            if (data != null)
            {
                bool us = data.Username.Equals(log.Username) && DecryptedPassword(data.Password).Equals(log.Password);
                if (us)
                {
                    var identity = new ClaimsIdentity(new[] {new Claim(ClaimTypes.Name, log.Username)},
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    HttpContext.Session.SetString("Username", log.Username);
                    return RedirectToAction("Home","Dashboard");
                }
                else
                {
                    TempData["ErrorPassword"] = "Invalid Password";
                }
            }
            else
            {
                TempData["ErrorUsername"] = "Invalid Username";
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedcookies = Request.Cookies.Keys;
            foreach (var cookie in storedcookies)
            {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("SignIn");
        }

    }
}
