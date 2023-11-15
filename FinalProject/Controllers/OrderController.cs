using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
