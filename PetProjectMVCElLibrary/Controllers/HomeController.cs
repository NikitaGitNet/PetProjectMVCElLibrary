using Microsoft.AspNetCore.Mvc;

namespace PetProjectMVCElLibrary.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
