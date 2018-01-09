using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using IS8012.AyonChoudhury.LabProject.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using System.Data.Entity.Migrations;

namespace IS8012.AyonChoudhury.LabProject
{
    public class TodoHub : Hub
    {
       private ApplicationDbContext db = new ApplicationDbContext();
        public void GetIndexData()
        {
           
            var data = db.TodoItems.Include(i => i.Category).ToList();
            var response = JsonConvert.SerializeObject(data, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
          


            Clients.Caller.getIndexPage(response);
        }
        public void CreateForm(string values)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var model = JsonConvert.DeserializeObject<TodoItem>(values);
            db.TodoItems.Add(model);
            db.SaveChanges();

            var reload = db.TodoItems.Include(x => x.Category)
                .FirstOrDefault(x => x.Id == model.Id);
            var response = JsonConvert.SerializeObject(reload, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            Clients.All.CreateTask(response);
        }
        public void DeleteTask(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var idInt = Convert.ToInt32(id);
            TodoItem todoitem = db.TodoItems.Find(idInt);
            db.TodoItems.Remove(todoitem);
            db.SaveChanges();

            Clients.All.DeleteTask(id);
        }
        public void CompleteTask(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var idInt = Convert.ToInt32(id);
            TodoItem todoitem = db.TodoItems.Find(idInt);
            todoitem.DateCompleted = DateTime.Now;
            db.TodoItems.AddOrUpdate(todoitem);
            db.SaveChanges();

            Clients.All.CompletedTask(id);
        }
        public void GetEditTask(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var idInt = Convert.ToInt32(id);
            var edittodoList = db.TodoItems.Find(idInt);

            var response = JsonConvert.SerializeObject(edittodoList, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            Clients.Caller.EditForm(response);
        }
        public void SaveEdit(int Id, string TaskName, string CompletedDate)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var todoitem = db.TodoItems.First(x => x.Id == Id);
            todoitem.Task = TaskName;
            todoitem.DateCompleted = ((String.IsNullOrEmpty(CompletedDate) || CompletedDate.Equals("null")) ? (DateTime?)null : Convert.ToDateTime(CompletedDate));
         
           

            db.Entry(todoitem).State = EntityState.Modified;
            db.SaveChanges();


            var data = db.TodoItems.Include(i => i.Category).ToList();
            var response = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            Clients.All.SaveEditForm(response);
        }
    }
}