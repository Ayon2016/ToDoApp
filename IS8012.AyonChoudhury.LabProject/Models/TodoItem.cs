using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IS8012.AyonChoudhury.LabProject.Models
{
    public class TodoItem
    {
    

        public int Id { get; set; }
        public String Task { get; set; }
        public DateTime? DateCompleted { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public bool MarkCompleted()
        {
            DateTime CompletedDate = DateTime.Now;
            DateCompleted = CompletedDate;
            return DateCompleted != null;
        }
      
    }
   
}