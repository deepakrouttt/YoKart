using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using YoKart.Services;

namespace YoKart.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult privacy()
        {
            return View();
        }

    }
}
