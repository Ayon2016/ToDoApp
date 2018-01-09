using IS8012.AyonChoudhury.LabProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IS8012.AyonChoudhury.LabProject.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var allLists = db.TodoItems.OrderBy(x => x.Task).ToList();
            return View(allLists);
            //return RedirectToAction("IndexSignalR");
        }
        public ActionResult DummyView()
        {
            var allLists = db.TodoItems.OrderBy(x => x.Task).ToList();
            return View(allLists);
        }
        public ActionResult JqAjax()
        {
            var allLists = db.TodoItems.OrderBy(x => x.Task).ToList();
            return View(allLists);
        }
        public ActionResult responsive()
        {
            var allLists = db.TodoItems.OrderBy(x => x.Task).ToList();
            return View(allLists);
        }
        public ActionResult location()
        {
            return View();
        }
        public ActionResult dynamic()
        {
            return View();
        }
        public ActionResult IndexSignalR()
        {
            return View();
        }
    }
}