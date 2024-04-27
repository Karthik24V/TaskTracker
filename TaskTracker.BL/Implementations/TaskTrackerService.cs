using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TaskTracker.BL.Interfaces;
using TaskTracker.BL.Models;
using TaskTracker.DL.Interfaces;
using TaskTracker.DL.Models;

namespace TaskTracker.BL.Implementations
{
    public class TaskTrackerService : ITaskTrackerService
    {
        private readonly ITaskTrackerRepository _taskRepository;

        public TaskTrackerService(ITaskTrackerRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TodoItemDTO>> GetAllTasks()
        {
            try
            {
                List<TodoItemDTO> tasks = new List<TodoItemDTO>();
                var allTasks = await _taskRepository.GetAllTasks();
                if (allTasks.Any())
                {
                    foreach (var task in allTasks)
                    {

                        var activities = await _taskRepository.GetActivitiesByTaskId(task.Id);
                        TodoItemDTO todo = new TodoItemDTO
                        {
                            Tags = task.Tags,
                            TaskName = task.TaskName,
                            AssignedTo = task.AssignedTo,
                            DueDate = task.DueDate,
                            Color = task.Color,
                            Id = task.Id,
                            Status = task.Status,
                            IsProcessed = task.IsProcessed,
                            AssignedToEmail = task.AssignedToEmail,
                            Activities = activities.Any() ? activities.ToList() : new List<ActivityDTO>()
                        };
                        tasks.Add(todo);
                    }
                }
                return tasks;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get all tasks.", ex);
            }
        }

        public async Task<TodoItemDTO> GetTaskById(int id)
        {
            try
            {
                TodoItemDTO todoTask = new TodoItemDTO();
                var task = await _taskRepository.GetTaskById(id);
                if (task != null)
                {
                    var activities =await _taskRepository.GetActivitiesByTaskId(task.Id);
                    todoTask.Tags = task.Tags;
                    todoTask.TaskName = task.TaskName;
                    todoTask.AssignedTo = task.AssignedTo;
                    todoTask.DueDate = task.DueDate;
                    todoTask.Color = task.Color;
                    todoTask.Id = task.Id;
                    todoTask.Status = task.Status;
                    todoTask.AssignedToEmail = task.AssignedToEmail;
                    todoTask.IsProcessed = task.IsProcessed;
                    todoTask.Activities = activities.Any() ? activities.ToList() : new List<ActivityDTO>();
                }
                return todoTask;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get task with ID: {id}.", ex);
            }
        }

        public async Task<bool> AddTasks(IEnumerable<TodoItemDTO> tasks)
        {
            try
            {
                var todoItems = tasks.Select(s =>
                {
                    var todo = new TodoItem();
                    todo.TaskName = s.TaskName;
                    todo.Tags = s.Tags;
                    todo.Status = s.Status;
                    todo.Color = s.Color;
                    todo.AssignedTo = s.AssignedTo;
                    todo.DueDate = s.DueDate ?? DateTime.MinValue;
                    todo.IsProcessed = false;
                    todo.AssignedToEmail = s.AssignedToEmail;
                    return todo;
                }).ToList();

                var result = await _taskRepository.CreateTask(todoItems);

                if (result.Any())
                {
                    foreach (var task in todoItems)
                    {
                        if (tasks.Any(t => t.Id == task.Id && t.Activities != null && t.Activities.Any()))
                        {
                            var activities = tasks.First(t => t.Id == task.Id).Activities;

                            await _taskRepository.AddActivities(task.Id, activities);
                        }
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add tasks.", ex);
            }
        }

        public async Task<bool> UpdateTask(TodoItemDTO task)
        {
            try
            {
               var existingTask =await _taskRepository.GetTaskById(task.Id);
                if(task == null)
                {
                    throw new Exception("Task Does not exist");
                }
                existingTask.Status = task.Status;
                existingTask.DueDate = task.DueDate ?? DateTime.MinValue;
                existingTask.TaskName = task.TaskName;
                existingTask.Tags = task.Tags;
                existingTask.Color = task.Color;
                existingTask.AssignedTo = task.AssignedTo;
                existingTask.AssignedToEmail = task.AssignedToEmail;
                existingTask.IsProcessed = task.IsProcessed;
                return await _taskRepository.UpdateTask(existingTask);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update task.", ex);
            }
        }

        public async Task<bool> DeleteTask(int id)
        {
            try
            {
                return await _taskRepository.DeleteTask(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete task with ID: {id}.", ex);
            }
        }

        public async Task<bool> MarkTaskAsComplete(int taskId)
        {
            try
            {
                var task = await _taskRepository.GetTaskById(taskId);
                if (task == null)
                {
                    throw new ArgumentException($"Task with ID {taskId} not found.");
                }

                task.Status = "Completed";

                return await _taskRepository.UpdateTask(task);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to mark task with ID {taskId} as complete.", ex);
            }
        }

        public async Task<IEnumerable<TodoItemDTO>> SearchTasks(SearchParametersDTO searchParameters)
        {

            try
            {
                List<TodoItemDTO> todoItems = new List<TodoItemDTO>();
                var tasks = await _taskRepository.GetAllTasks();

                if (!string.IsNullOrEmpty(searchParameters.TaskName))
                {
                    tasks = tasks.Where(t => t.TaskName.ToLower().Contains(searchParameters.TaskName.ToLower()));
                }

                if (searchParameters.Tags != null && searchParameters.Tags.Any())
                {
                    tasks = tasks.Where(t => t.Tags.ToLower().Split(',', StringSplitOptions.RemoveEmptyEntries).Intersect(searchParameters.Tags).Any());
                }

                if (searchParameters.DueDateFrom.HasValue)
                {
                    tasks = tasks.Where(t => t.DueDate >= searchParameters.DueDateFrom);
                }

                if (searchParameters.DueDateTo.HasValue)
                {
                    tasks = tasks.Where(t => t.DueDate <= searchParameters.DueDateTo);
                }

                if (searchParameters.Statuses != null && searchParameters.Statuses.Any())
                {
                    tasks = tasks.Where(t => searchParameters.Statuses.Contains(t.Status));
                }
                if (tasks.Any())
                {
                    foreach (var task in tasks)
                    {

                        var activities = _taskRepository.GetActivitiesByTaskId(task.Id).Result;
                        TodoItemDTO todo = new TodoItemDTO
                        {
                            Tags = task.Tags,
                            TaskName = task.TaskName,
                            AssignedTo = task.AssignedTo,
                            DueDate = task.DueDate,
                            Color = task.Color,
                            Id = task.Id,
                            Status = task.Status,
                            AssignedToEmail = task.AssignedToEmail,
                            IsProcessed = task.IsProcessed,
                            Activities = activities.Any() ? activities.ToList() : new List<ActivityDTO>()
                        };
                        todoItems.Add(todo);
                    }
                }
                return todoItems;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to search tasks.", ex);
            }
        }
        public async Task AddActivities(int taskId, IEnumerable<ActivityDTO> activities)
        {
            await _taskRepository.AddActivities(taskId, activities);
        }
    }
}
