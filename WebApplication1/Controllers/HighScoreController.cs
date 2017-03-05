using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Frame.DataAccess;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HighScoreController : Controller
    {
        public HighScoreController()
        {

        }
        public ActionResult Index()
        {
            var provider = new SQLiteDataProvider();
            var results = provider.SelectAll();
            return View(results);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(HighScore highScore)
        {
            if (ModelState.IsValid)
            {
                var provider = new SQLiteDataProvider();
                provider.Insert(highScore);
                return RedirectToAction("Index");
            }
            return View(highScore);
        }
        [HttpPost]
        public ActionResult Delete(HighScore highScore)
        {
            var provider = new SQLiteDataProvider();
            provider.Delete(highScore);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult List()
        {
            var provider = new SQLiteDataProvider();
            var results = provider.SelectAll();
            return PartialView(results);
        }
    }
}