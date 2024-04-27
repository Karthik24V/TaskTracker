using TaskTracker.DL.Models;

public class TaskTrackerResponse
{
    public int Id { get; set; }
    public string TaskName { get; set; }
    public string Tags { get; set; }
    public string DueDate { get; set; }
    public string Color { get; set; }
    public string AssignedTo { get; set; }
    public string Status { get; set; }
    public List<ActivityDTO> Activities { get; set; }
    public string AssignedToEmail { get; set; }
    public bool IsProcessed { get; set; }
}