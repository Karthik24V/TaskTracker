using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.BL.Models;
using TaskTracker.DL.Models;

namespace TaskTracker.BL.Interfaces
{
    public interface ITaskTrackerService
    {
        Task<IEnumerable<TodoItemDTO>> GetAllTasks();
        Task<TodoItemDTO> GetTaskById(int id);
        Task<bool> AddTasks(IEnumerable<TodoItemDTO> task);
        Task<bool> UpdateTask(TodoItemDTO task);
        Task<bool> DeleteTask(int id);
        Task<bool> MarkTaskAsComplete(int taskId);
        Task<IEnumerable<TodoItemDTO>> SearchTasks(SearchParametersDTO searchParameters);
        Task AddActivities(int taskId, IEnumerable<ActivityDTO> activities);
    }
}
