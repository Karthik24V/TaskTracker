using MudBlazor;
using System.Text.Json.Serialization;
using TaskTracker.DL.Models;
using TaskTracker.UI.Enum;

namespace TaskTracker.UI.Models
{
    public class TaskTrackerModel
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public List<string> Tags { get; set; }
        public DateTime DueDate { get; set; }
        public string Color { get; set; }
        public string AssignedTo { get; set; }
        public StatusEnum Status { get; set; }
        public List<ActivityDTO>? Activities { get; set; }

    }
    public class Element
    {
        public int Id { get; set; }
        public string Group { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }

        [JsonPropertyName("small")]
        public string Sign { get; set; }
        public double Molar { get; set; }
        public IList<int> Electrons { get; set; }
        public List<ActivityData> ActivityData { get; set; }

        public override string ToString()
        {
            return $"{Sign} - {Name}";
        }
    }
}
