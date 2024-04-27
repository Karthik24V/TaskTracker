using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskTracker.BL.Interfaces;
using TaskTracker.BL.Models;
using TaskTracker.DL.Models;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskTrackerService _taskTrackerService;

        public TasksController(ITaskTrackerService taskTrackerService)
        {
            _taskTrackerService = taskTrackerService;
        }

        [HttpGet("getTasks")]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTasks()
        {
            try
            {
                var tasks =await _taskTrackerService.GetAllTasks();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getTask/{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            try
            {
                var task = await _taskTrackerService.GetTaskById(id);

                if (task == null)
                {
                    return NotFound();
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("createTasks")]
        public async Task<IActionResult> AddTasks([FromBody]List<TodoItemDTO> todoItems)
        {
            try
            {
                if (todoItems == null || !todoItems.Any())
                {
                    return BadRequest("No tasks provided.");
                }

                await _taskTrackerService.AddTasks(todoItems);

                return Ok("Tasks added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("updateTask/{id}")]
        public async Task<IActionResult> PutTask(int id, TodoItemDTO task)
        {
            try
            {
                if (id != task.Id)
                {
                    return BadRequest();
                }

                await _taskTrackerService.UpdateTask(task);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("RemoveTask/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var existingTask = await _taskTrackerService.GetTaskById(id);

                if (existingTask == null)
                {
                    return NotFound();
                }

                await _taskTrackerService.DeleteTask(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("complete/{taskId}")]
        public async Task<IActionResult> MarkTaskAsComplete(int taskId)
        {
            try
            {
                await _taskTrackerService.MarkTaskAsComplete(taskId);
                return Ok("Task marked as complete.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to mark task as complete: {ex.Message}");
            }
        }

        [HttpGet("searchTasks")]
        public async Task<IActionResult> SearchTasks([FromQuery] SearchParametersDTO searchParameters)
        {
            try
            {
                var tasks = await _taskTrackerService.SearchTasks(searchParameters);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to search tasks: {ex.Message}");
            }
        }

        [HttpPost("addActivities/{taskId}")]
        public async Task<IActionResult> AddActivities(int taskId, List<ActivityDTO> activities)
        {
            try
            {
                var existingTask = await _taskTrackerService.GetTaskById(taskId);
                if (existingTask == null)
                {
                    return NotFound("Task not found.");
                }

                await _taskTrackerService.AddActivities(taskId, activities);

                return Ok("Activities added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to add activities: {ex.Message}");
            }
        }

    }
}
