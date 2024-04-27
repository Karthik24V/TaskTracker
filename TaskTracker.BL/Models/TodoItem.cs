using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DL.Models;

namespace TaskTracker.BL.Models
{
    public class TodoItemDTO
    {
        public int Id { get; set; } 
        public string? TaskName { get; set; } 
        public string? Tags { get; set; } 
        public DateTime? DueDate { get; set; }
        public string? Color { get; set; } 
        public string? AssignedTo { get; set; } 
        public string? AssignedToEmail { get; set; }
        public string? Status { get; set; }
        public bool IsProcessed {  get; set; }
        public List<ActivityDTO>? Activities { get; set; }
    }
}
