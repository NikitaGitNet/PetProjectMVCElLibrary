using Microsoft.AspNetCore.Mvc;

namespace PetProjectMVCElLibrary.Areas.Admin.Controllers
{
    /// <summary>
    /// Контроллер отвечающий за вывод данных/информации в панель администратора
    /// </summary>
    [Area("Admin")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Метод возвращает представление панели администратора
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
