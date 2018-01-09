using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IS8012.AyonChoudhury.LabProject.Models;
using Newtonsoft.Json;
using System.Data.Entity.Migrations;

namespace IS8012.AyonChoudhury.LabProject.Controllers
{
    public class TodoItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TodoItems
        //public ActionResult Index()
        //{
        //    var todoItems = db.TodoItems.Include(t => t.Category);
        //    return View(todoItems.ToList());
        //}
        public ActionResult Index()
        {
            var data = db.TodoItems.Include(i => i.Category).ToList();
            var items = JsonConvert.SerializeObject(data, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Content(items);
        }

        // GET: TodoItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodoItem todoItem = db.TodoItems.Find(id);
            if (todoItem == null)
            {
                return HttpNotFound();
            }
            return View(todoItem);
        }

        // GET: TodoItems/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");

            if (Request.IsAjaxRequest())
            {
                return PartialView("_create");
            }
            else
            {
                return View();
            }
        }

        // POST: TodoItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
    
        public ActionResult Create([Bind(Include = "Id,Task,DateCompleted,CategoryId")] TodoItem todoItem)
        {
            //  if (ModelState.IsValid)
            //{
            //db.TodoItems.Add(todoItem);
            //db.SaveChanges();
            // return RedirectToAction("Index");
            // }

            // ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", todoItem.CategoryId);
            // return View(todoItem);
            //  }
           

            if (ModelState.IsValid)
            {
          
                db.TodoItems.Add(todoItem);
                db.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    // reload book
                    var reloadeprop = db.TodoItems
                                            .Include(x => x.Category)
                                            .FirstOrDefault(x => x.Id == todoItem.Id);
                    return Content(JsonConvert.SerializeObject(reloadeprop, Formatting.Indented,
                            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                }
              
               
                else
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", todoItem.CategoryId);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_create", todoItem);
            }
            else
            {
                return View(todoItem);
            }
        }
        // GET: TodoItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodoItem todoItem = db.TodoItems.Find(id);
            if (todoItem == null)
            {
                return HttpNotFound();
            }
            if(Request.IsAjaxRequest())
            {
                var toReturn = db.TodoItems.Include(x => x.Category).FirstOrDefault(x => x.Id == id);

                return Content(JsonConvert.SerializeObject(toReturn, Formatting.Indented,
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", todoItem.CategoryId);
            return View(todoItem);
        }

        // POST: TodoItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Task,DateCompleted,CategoryId")] TodoItem todoItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(todoItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", todoItem.CategoryId);
            return View(todoItem);
        }

        public ActionResult EditSave(int Id, string TaskName, string CompletedDate)
        {
            var item = db.TodoItems.First(x => x.Id == Id);
            item.Task = TaskName;
            item.DateCompleted = ((String.IsNullOrEmpty(CompletedDate) || CompletedDate.Equals("null")) ? (DateTime?)null : Convert.ToDateTime(CompletedDate));

            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();

            var currentEditedItem = db.TodoItems.Include(x => x.Category).ToList();
            return Content(JsonConvert.SerializeObject(currentEditedItem, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }



        // GET: TodoItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodoItem todoItem = db.TodoItems.Find(id);
            if (todoItem == null)
            {
                return HttpNotFound();
            }
            return View(todoItem);
        }

        // POST: TodoItems/Delete/5
        [HttpPost]

        public ActionResult DeleteConfirmed(int id)
        {
            TodoItem listing = db.TodoItems.Find(id);
            db.TodoItems.Remove(listing);
            db.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Content("");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Completed(int id)
        {
            TodoItem todoItem = db.TodoItems.Find(id);
           if (todoItem == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            todoItem.DateCompleted = DateTime.Now;
            db.TodoItems.AddOrUpdate(todoItem);
            db.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Content("");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        //public bool MarkCompleted()
        //{
        //    DateTime CompletedDate = DateTime.Now;
        //    DateCompleted = CompletedDate;
        //    return DateCompleted != null;
        //}
    }
}
