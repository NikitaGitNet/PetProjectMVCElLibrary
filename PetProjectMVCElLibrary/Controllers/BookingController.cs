using Microsoft.AspNetCore.Mvc;

namespace PetProjectMVCElLibrary.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
