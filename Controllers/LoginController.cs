using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TFI_PAD.Models;

namespace TFI_PAD.Controllers
{
    public class LoginController : Controller
    {
        private BibliotecaEntities _context = new BibliotecaEntities();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login user)
        {
            if (ModelState.IsValid)
            {
                var IsValidUser = false;
                var loginUser = _context.Logins.FirstOrDefault(u => u.Username == user.Username);
                if (loginUser != null && user.Password == loginUser.Password)
                    IsValidUser = true;


                if (IsValidUser)
                {
                    var role = loginUser.Profesor == 0 ? "Alumno" : "Profesor";
                    SignIn($"{role}:{user.Username}", false);
                    FormsAuthentication.RedirectFromLoginPage($"{role}:{user.Username}", false);
                }
            }
            ModelState.AddModelError("InvalidLogin", "Contraseña o Usuario invalido");
            return View();
        }

        [NonAction]
        public void SignIn(string username, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();
            TimeSpan expirationTimeSpan = FormsAuthentication.Timeout;

            var ticket = new FormsAuthenticationTicket(
                1,
                username,
                now,
                now.Add(expirationTimeSpan),
                createPersistentCookie,
                "",
                FormsAuthentication.FormsCookiePath
            );

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                encryptedTicket)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath
            };

            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }
            Response.Cookies.Add(cookie);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
