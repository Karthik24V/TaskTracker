using AutoMapper;
using Microsoft.VisualBasic;
using MudBlazor.Extensions;
using Newtonsoft.Json;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TaskTracker.BL.Models;
using TaskTracker.DL.Models;
using TaskTracker.UI.Enum;
using TaskTracker.UI.Models;
using static MudBlazor.CategoryTypes;

public class TaskServices
{
    private readonly HttpClient _httpClient;

    public TaskServices(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.BaseAddress = new Uri("https://localhost:44327/");
    }

    public async Task<List<TaskTrackerModel>> SearchTasks(SearchData searchData)
    {
        HttpClient client = new HttpClient();
        try
        {
            var queryString = BuildQueryString(searchData);

            var url = $"api/Tasks/searchTasks{queryString}";
            //var data = await _httpClient.GetAsync("api/Tasks/test");
            var list = await _httpClient.GetFromJsonAsync<List<TodoItemDTO>>(url);
            return list.Select(l => new TaskTrackerModel
            {
                Id = l.Id,
                TaskName = l.TaskName,
                Tags = l.Tags.Split(",", StringSplitOptions.None).ToList(),
                DueDate = l.DueDate ?? DateTime.MinValue,
                AssignedTo = l.AssignedTo,
                Status = (StatusEnum)Enum.Parse(typeof(StatusEnum), l.Status),
                Activities = l.Activities,
                Color = l.Color,
            }).ToList();

        }
        catch (Exception ex)
        {
            client.Dispose();
            return new List<TaskTrackerModel>();
        }
    }
    public async Task<string> AddTasks(List<TaskTrackerModel> tasks)
    {
        var data = tasks.Select(t => new TaskTrackerResponse
        {
            Id = t.Id,
            TaskName = t.TaskName,
            Tags = string.Join(",", t.Tags),
            DueDate = DateTime.Now.ToIsoDateString(),
            Color = t.Color,
            AssignedTo = t.AssignedTo,
            Status = t.Status.ToString(),
            IsProcessed = true,
            AssignedToEmail = string.Empty,
        }).ToList(); 
        var response = await _httpClient.PostAsJsonAsync("api/Tasks/createTasks", data);
        response.EnsureSuccessStatusCode();
        return "Task created successfully";
    }
    public async Task<string> AddActivity(List<ActivityDTO> activities,int taskId)
    {
        var data = activities.Select(x => new 
        {
           Id = x.Id,
           ActivityDate = DateTime.Now.ToIsoDateString(),
           ActivityDescription = x.ActivityDescription,
           DoneBy = x.DoneBy,
           TodoItemId = x.TodoItemId,
        });
        var response = await _httpClient.PostAsJsonAsync($"api/Tasks/addActivities/{taskId}", data);
        response.EnsureSuccessStatusCode();
        return "Task created successfully";
    }

    public async Task MarkTaskAsComplete(int taskId)
    {
        var response = await _httpClient.PostAsync($"api/Tasks/complete/{taskId}", null);
        response.EnsureSuccessStatusCode();
    }
    public async Task<TaskTrackerModel> GetTaskWithId(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<TaskTrackerResponse>($"api/Tasks/getTask/{id}");
        var data = new TaskTrackerModel
        {
            Id = response.Id,
            TaskName = response.TaskName,
            Tags = response.Tags?.Split(",", StringSplitOptions.None).ToList(),
            DueDate = Convert.ToDateTime(response.DueDate),
            Activities = response.Activities,
            AssignedTo = response.AssignedTo,
            Color = response.Color,
            Status = (StatusEnum)Enum.Parse(typeof(StatusEnum), response.Status)
        };
        return data;
    }

    public async Task<List<TodoItemDTO>> GetTasks()
    {
        return await _httpClient.GetFromJsonAsync<List<TodoItemDTO>>("api/Tasks/getTasks");
    }

    public async Task<string> UpdateTask(TaskTrackerModel task)
    {
        var data = new TaskTrackerResponse
        {
            Id = task.Id,
            TaskName = task.TaskName,
            Tags = string.Join(",", task.Tags),
            DueDate = task.DueDate.ToIsoDateString(),
            Color = task.Color,
            AssignedTo = task.AssignedTo,
            Status = task.Status.ToString(),
            IsProcessed = true,
            AssignedToEmail = string.Empty,
            Activities = new List<ActivityDTO>(),
        };
        var response = await _httpClient.PutAsJsonAsync($"api/Tasks/updateTask/{data.Id}", data);
        response.EnsureSuccessStatusCode();
        return "Task Updated Successfully";
    }

    public async Task<string> DeleteTask(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Tasks/RemoveTask/{id}");
        response.EnsureSuccessStatusCode();
        return "Task Deleted Successfully";
    }
    private string BuildQueryString(SearchData searchData)
    {
        var queryStringBuilder = new StringBuilder();

        // Append TaskName if not null or empty
        if (!string.IsNullOrEmpty(searchData.TaskName))
        {
            queryStringBuilder.Append($"TaskName={Uri.EscapeDataString(searchData.TaskName)}&");
        }

        // Append Tags if not null or empty
        if (searchData.Tags != null && searchData.Tags.Any())
        {
            foreach (var tag in searchData.Tags)
            {
                queryStringBuilder.Append($"Tags={Uri.EscapeDataString(tag)}&");
            }
        }

        // Append DueDateFrom if not null
        if (searchData.DueDateFrom.HasValue)
        {
            queryStringBuilder.Append($"DueDateFrom={searchData.DueDateFrom.Value:yyyy-MM-ddTHH:mm:ss}&");
        }

        // Append DueDateTo if not null
        if (searchData.DueDateTo.HasValue)
        {
            queryStringBuilder.Append($"DueDateTo={searchData.DueDateTo.Value:yyyy-MM-ddTHH:mm:ss}&");
        }

        // Append Statuses if not null or empty
        if (searchData.Statuses != null && searchData.Statuses.Any())
        {
            foreach (var status in searchData.Statuses)
            {
                queryStringBuilder.Append($"Statuses={Uri.EscapeDataString(status)}&");
            }
        }

        // Remove the trailing '&' if present
        if (queryStringBuilder.Length > 0)
        {
            queryStringBuilder.Length--; // Remove the last character (the trailing '&')
        }

        // Convert StringBuilder to string
        return queryStringBuilder.ToString();
    }

    private void AppendQueryParam(StringBuilder queryStringBuilder, string key, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            queryStringBuilder.Append($"&{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}");
        }
        else
        {
            queryStringBuilder.Append($"&{Uri.EscapeDataString(key)}={Uri.EscapeDataString(string.Empty)}");
        }
    }
}
