using Microsoft.AspNetCore.Mvc;

namespace EcomRest.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult ProductList()
        {
            return View();
        }

        public IActionResult AddProduct()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
       

    }
}
