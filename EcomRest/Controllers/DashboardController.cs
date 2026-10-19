
using Microsoft.AspNetCore.Mvc;

namespace EcomRest.Controllers
{
    public class DashboardController : Controller
    {
        // Admin Dashboard
        public IActionResult AdminDashboard()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int roleId = Convert.ToInt32(
                HttpContext.Session.GetString("RoleId"));

            if (roleId != 1)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // Regular User Dashboard
        public IActionResult UserDashboard()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int roleId = Convert.ToInt32(
                HttpContext.Session.GetString("RoleId"));

            if (roleId == 1)
            {
                return RedirectToAction("AdminDashboard", "Dashboard");
            }

            return View();
        }
    }
}
