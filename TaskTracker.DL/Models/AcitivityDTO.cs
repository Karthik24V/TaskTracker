using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.DL.Models
{
    public class ActivityDTO
    {
        public int Id { get; set; }
        public int TodoItemId { get; set; }
        public DateTime ActivityDate { get; set; }
        public string DoneBy { get; set; }
        public string ActivityDescription { get; set; }
    }

}
