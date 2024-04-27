using TaskTracker.DL.Models;

namespace TaskTracker.DL.Interfaces
{
    public interface ITaskTrackerRepository
    {
        Task<IEnumerable<TodoItem>> GetAllTasks();
        Task<TodoItem> GetTaskById(int id);
        Task<IEnumerable<int>> CreateTask(IEnumerable<TodoItem> task);
        Task<bool> UpdateTask(TodoItem task);
        Task<bool> DeleteTask(int id);
        Task<List<TodoItem>> SearchTasks(SearchParameters searchParameters);
        Task<bool> AddActivities(int taskId, IEnumerable<ActivityDTO> activities);
        Task<IEnumerable<ActivityDTO>> GetActivitiesByTaskId(int taskId);
        Task<bool> MarkTaskAsProcessed(int taskId);
        Task<IEnumerable<TodoItem>> GetNewTasks();
    }
}