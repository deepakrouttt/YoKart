using Microsoft.AspNetCore.Mvc;

namespace YoKart.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
