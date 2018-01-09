using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IS8012.AyonChoudhury.LabProject.Models
{
    public class Category
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public virtual List<TodoItem> TodoItems { get; set; }
    }
}