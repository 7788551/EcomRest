using Microsoft.AspNetCore.Mvc;

namespace EcomRest.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            ViewBag.Message = "Create your New Acoount";
            ViewData["Title"] = "Register";
            return View();
        }

        public IActionResult Login()
        {
            ViewBag.Message = "Please Enter your Login Credentials,";
            ViewData["Title"] = "Login";
            return View();
        }
    }
}
