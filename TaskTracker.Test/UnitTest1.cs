using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Controllers;
using TaskTracker.BL.Interfaces;
using TaskTracker.BL.Models;

namespace TaskTracker.Test
{
    [TestFixture]
    public class TasksControllerTests
    {
        private Mock<ITaskTrackerService> _taskTrackerServiceMock;
        private TasksController _controller;

        [SetUp]
        public void Setup()
        {
            _taskTrackerServiceMock = new Mock<ITaskTrackerService>();
            _controller = new TasksController(_taskTrackerServiceMock.Object);
        }

        [Test]
        public async Task GetTasks_ReturnsOkResult_WithListOfTasks()
        {
            // Arrange
            var expectedTasks = new List<TodoItemDTO> { /* Populate with test data */ };
            _taskTrackerServiceMock.Setup(s => s.GetAllTasks()).ReturnsAsync(expectedTasks);

            // Act
            var result = await _controller.GetTasks();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<IEnumerable<TodoItemDTO>>(okResult.Value);
            var actualTasks = okResult.Value as IEnumerable<TodoItemDTO>;
            Assert.AreEqual(expectedTasks.Count(), actualTasks.Count());
            // Add more assertions as needed
        }

        // Write similar test methods for other API endpoints

    }

    [TestFixture]
    public class TaskTrackerServiceTests
    {
        private Mock<ITaskTrackerRepository> _taskRepositoryMock;
        private TaskTrackerService _service;

        [SetUp]
        public void Setup()
        {
            _taskRepositoryMock = new Mock<ITaskTrackerRepository>();
            _service = new TaskTrackerService(_taskRepositoryMock.Object);
        }

        [Test]
        public async Task GetAllTasks_ReturnsListOfTasks()
        {
            // Arrange
            var expectedTasks = new List<TodoItemDTO> { /* Populate with test data */ };
            _taskRepositoryMock.Setup(r => r.GetAllTasks()).ReturnsAsync(expectedTasks);

            // Act
            var actualTasks = await _service.GetAllTasks();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<TodoItemDTO>>(actualTasks);
            Assert.AreEqual(expectedTasks.Count(), actualTasks.Count());
            // Add more assertions as needed
        }

        // Write similar test methods for other service methods

    }
}
