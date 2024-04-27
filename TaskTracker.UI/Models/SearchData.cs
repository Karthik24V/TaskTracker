namespace TaskTracker.UI.Models
{
    public class SearchData
    {
        public string TaskName { get; set; }
        public string[] Tags { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public string[] Statuses { get; set; }
    }
}
