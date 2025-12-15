using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace lista8final.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
