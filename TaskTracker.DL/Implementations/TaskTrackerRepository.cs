using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskTracker.DL.Interfaces;
using TaskTracker.DL.Models;

namespace TaskTracker.DL.Implementations
{
    public class TaskTrackerRepository : ITaskTrackerRepository
    {
        private readonly TaskDbContext _context;

        public TaskTrackerRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<int>> CreateTask(IEnumerable<TodoItem> tasks)
        {
            try
            {
                _context.TodoItems.AddRange(tasks);
                await _context.SaveChangesAsync();
                return tasks.Select(s=>s.Id);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create tasks.", ex);
            }
        }

        public async Task<bool> DeleteTask(int id)
        {
            try
            {
                var task = await _context.TodoItems.FindAsync(id);
                if (task == null)
                {
                    throw new Exception($"Task with ID: {id} not found.");
                }
                var activities = await _context.Activities.Where(a => a.TodoItemId == id).ToListAsync();
                _context.Activities.RemoveRange(activities);
                _context.TodoItems.Remove(task);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete task with ID: {id}.", ex);
            }
        }


        public async Task<IEnumerable<TodoItem>> GetAllTasks()
        {
            try
            {
                return await _context.TodoItems.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve all tasks with activities.", ex);
            }
        }

        public async Task<TodoItem> GetTaskById(int id)
        {
            try
            {
                return await _context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve task with ID {id} with activities.", ex);
            }
        }

        public async Task<bool> UpdateTask(TodoItem task)
        {
            try
            {
                _context.Update(task);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update task with ID: {task.Id}.", ex);
            }
        }

        public async Task<List<TodoItem>> SearchTasks(SearchParameters searchParameters)
        {
            try
            {
                var query = _context.TodoItems.AsQueryable();

                if (!string.IsNullOrEmpty(searchParameters.TaskName))
                {
                    query = query.Where(t => t.TaskName.ToLower().Contains(searchParameters.TaskName.ToLower()));
                }

                if (searchParameters.Tags != null && searchParameters.Tags.Any())
                {
                    query = query.Where(t => searchParameters.Tags.All(tag => t.Tags.Contains(tag)));
                }

                if (searchParameters.DueDateFrom.HasValue)
                {
                    query = query.Where(t => t.DueDate >= searchParameters.DueDateFrom);
                }

                if (searchParameters.DueDateTo.HasValue)
                {
                    query = query.Where(t => t.DueDate <= searchParameters.DueDateTo);
                }

                if (searchParameters.Statuses != null && searchParameters.Statuses.Any())
                {
                    query = query.Where(t => searchParameters.Statuses.Contains(t.Status));
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to search tasks.", ex);
            }
        }

        public async Task<bool> AddActivities(int taskId, IEnumerable<ActivityDTO> activities)
        {
            try
            {
                var newActivities = activities.Select(a => new Activity
                {
                    TodoItemId = taskId,
                    ActivityDate = a.ActivityDate,
                    DoneBy = a.DoneBy,
                    ActivityDescription = a.ActivityDescription
                }).ToList();
                _context.Activities.AddRange(newActivities);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create tasks.", ex);
            }
        }

        public async Task<IEnumerable<ActivityDTO>> GetActivitiesByTaskId(int taskId)
        {
            try
            {
               var activities = _context.Activities.Where(s => s.TodoItemId == taskId).ToList();
                return activities.Select(s =>
                {
                    return new ActivityDTO
                    {
                        TodoItemId = s.TodoItemId,
                        ActivityDate = s.ActivityDate,
                        ActivityDescription = s.ActivityDescription,
                        DoneBy = s.DoneBy,
                        Id = s.Id
                    };
                }).ToList();
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to retrieve activities for task id {taskId}.", ex);
            }
        }

        public async Task<IEnumerable<TodoItem>> GetNewTasks()
        {
            // Return tasks that are not marked as processed
            return await _context.TodoItems.Where(t => !t.IsProcessed).ToListAsync();
        }

        public async Task<bool> MarkTaskAsProcessed(int taskId)
        {
            try
            {
                var task = await _context.TodoItems.FindAsync(taskId);
                if (task == null)
                {
                    throw new Exception($"Task with ID: {taskId} not found.");
                }

                // Mark the task as processed
                task.IsProcessed = true;

                // Update the task in the database
                _context.Update(task);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to mark task with ID: {taskId} as processed.", ex);
            }
        }

    }
}
