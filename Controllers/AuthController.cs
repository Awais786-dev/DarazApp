using Microsoft.AspNetCore.Mvc;

namespace DarazApp.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
