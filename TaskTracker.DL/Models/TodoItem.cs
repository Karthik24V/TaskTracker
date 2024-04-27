using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.DL.Models
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; } 
        public string TaskName { get; set; } 
        public string Tags { get; set; } 
        public DateTime DueDate { get; set; }
        public string Color { get; set; } 
        public string AssignedTo { get; set; } 
        public string AssignedToEmail {  get; set; }
        public string Status { get; set; }
        public bool IsProcessed { get; set; }
    }

    public class Activity
    {
        [Key]
        public int Id { get; set; }
        public DateTime ActivityDate { get; set; }
        public string DoneBy { get; set; }
        public string ActivityDescription { get; set; }

        public int TodoItemId { get; set; }
    }
}
