using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.BL.Models;
using TaskTracker.DL.Interfaces;
using Microsoft.Extensions.Hosting;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace TaskTracker.BL.Implementations
{
    public class TaskNotificationService : BackgroundService
    {
        private readonly ITaskTrackerRepository _taskRepository;
        private readonly ISendGridClient _sendGridClient;

        public TaskNotificationService(ITaskTrackerRepository taskRepository, ISendGridClient sendGridClient)
        {
            _taskRepository = taskRepository;
            _sendGridClient = sendGridClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);

                var newTasks = await _taskRepository.GetNewTasks();

                foreach (var task in newTasks)
                {
                    var taskDTO = new TodoItemDTO
                    {
                        TaskName = task.TaskName,
                        Id = task.Id,
                        Tags = task.Tags,
                        DueDate = task.DueDate,
                        Status = task.Status,
                        AssignedTo = task.AssignedTo,
                        AssignedToEmail = task.AssignedToEmail,
                        Color = task.Color,
                    };
                    await SendTaskNotificationEmail(taskDTO);

                    await _taskRepository.MarkTaskAsProcessed(taskDTO.Id);
                }
            }
        }

        private async Task SendTaskNotificationEmail(TodoItemDTO task)
        {
            try
            {
            var subject = $"New Task Assigned: {task.TaskName}";
            var body = $"You have been assigned a new task: {task.TaskName}. Please log in to the application to view the details.";

            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("sender@example.com", "TaskTracker Application"));
            msg.AddTo(new EmailAddress(task.AssignedToEmail, task.AssignedTo));
            msg.SetSubject(subject);
            msg.AddContent(MimeType.Html, body);

            var response = await _sendGridClient.SendEmailAsync(msg);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed to send email: {response.StatusCode}");
            }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email", ex);

            }
        }
    }
}
