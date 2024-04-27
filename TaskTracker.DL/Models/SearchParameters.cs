using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.DL.Models
{
    public class SearchParameters
    {
        public string TaskName { get; set; }
        public string[] Tags { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public string[] Statuses { get; set; }
    }
}
