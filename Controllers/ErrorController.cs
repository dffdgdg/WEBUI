using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/error/404")]
        public IActionResult NotFound()
        {
            return View("NotFound"); // Возвращает представление NotFound.cshtml
        }

        [Route("/error")]
        public IActionResult Error()
        {
            return View("Error"); // Возвращает представление Error.cshtml (если есть)
        }
    }
}