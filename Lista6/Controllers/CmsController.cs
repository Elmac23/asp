using Microsoft.AspNetCore.Mvc;

namespace Lista6.Controllers
{
    public class CmsController : Controller
    {
        [Route("cms/{*path}")]
        public IActionResult Page(string path)
        {
            if (string.IsNullOrEmpty(path)) path = "home";
            ViewData["CmsPath"] = path;

            if (path.Contains("alt"))
                ViewData["Layout"] = "_AltLayout";
            else
                ViewData["Layout"] = "_Layout";

            return View("Page");
        }
    }
}
