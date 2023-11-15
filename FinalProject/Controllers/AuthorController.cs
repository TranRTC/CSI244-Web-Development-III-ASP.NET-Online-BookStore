using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
