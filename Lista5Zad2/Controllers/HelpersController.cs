using Microsoft.AspNetCore.Mvc;
using Lista5Zad2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Lista5Zad2.Controllers
{
    public class HelpersController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = new HelpersDemo
            {
                Options = new List<SelectListItem>
                {
                    new SelectListItem("Opcja A", "A"),
                    new SelectListItem("Opcja B", "B"),
                    new SelectListItem("Opcja C", "C")
                },
                RawHtml = "<strong style=\"color:green\">To jest <em>surowy</em> HTML</strong>"
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(HelpersDemo model)
        {
            // Przyjêcie danych, proste przekierowanie do tej samej strony z modelem
            model.Options = new List<SelectListItem>
            {
                new SelectListItem("Opcja A", "A"),
                new SelectListItem("Opcja B", "B"),
                new SelectListItem("Opcja C", "C")
            };
            ViewBag.Message = "Dane przes³ane poprawnie.";
            return View(model);
        }
    }
}
