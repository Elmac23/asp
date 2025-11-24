using Microsoft.AspNetCore.Mvc;
using Lista6.Models;

namespace Lista6.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
