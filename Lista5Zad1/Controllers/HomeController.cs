using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lista5Zad1.Models;

namespace Lista5Zad1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult TaskScores()
        {
            var model = new TaskScoresViewModel
            {
                Scores = new int[10]
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaskScores(TaskScoresViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["ResultModel"] = model;
                return RedirectToAction("TaskScoresResult");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult TaskScoresResult()
        {
            var model = TempData["ResultModel"] as TaskScoresViewModel;
            if (model == null)
            {
                return RedirectToAction("TaskScores");
            }

            return View(model);
        }
    }
}