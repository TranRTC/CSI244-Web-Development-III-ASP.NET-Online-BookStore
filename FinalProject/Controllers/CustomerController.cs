using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
