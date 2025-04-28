using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Todo.Api.Controllers;
using Todo.Api.Model.Dtos;
using Todo.Api.Services;

namespace Todo.Tests.Controller
{
    public class TodoControllerTests
    {
        [Fact]
        public void TodoController_GetAllTodos_ReturnStatusOkWithListOfTodoResponse()
        {
            //Arrnage
            var mockService = new Mock<ITodoService>();
            var expectedTodos = new List<TodoResponse>
            {
                new TodoResponse { Id = 1, Title = "Task 1", Description = "Description 1", PercentOfComplete = 50, ExpirationDateTime = DateTime.Now },
                new TodoResponse { Id = 2, Title = "Task 2", Description = "Description 2", PercentOfComplete = 100, ExpirationDateTime = DateTime.Now }
            };

            mockService.Setup(service => service.GetAllTodos()).Returns(expectedTodos);
            var controller = new TodoController(mockService.Object);

            // Act
            var result = controller.GetAllTodos();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeAssignableTo<IEnumerable<TodoResponse>>()
                .Which.Should().HaveCount(2)
                .And.ContainSingle(t => t.Title == "Task 1")
                .And.ContainSingle(t => t.Title == "Task 2");
        }

        [Fact]
        public void TodoController_AddTodo_ReturnStatusCreated()
        {
            //Arrnage
            var mockService = new Mock<ITodoService>();
            var datetime = DateTime.Now;
            var todoToAdd = new TodoAddRquest()
            {
                Title = "Title 1",
                Description = "Description 1",
                PercentOfComplete = 0,
                ExpirationDateTime = datetime
            };
            var expectedTodo = new TodoResponse 
            { 
                Id = 1, 
                Title = "Task 1", 
                Description = "Description 1", 
                PercentOfComplete = 0, 
                ExpirationDateTime = datetime
            };
            mockService.Setup(service => service.AddTodo(todoToAdd)).Returns(expectedTodo);
            var controller = new TodoController(mockService.Object);

            //Act
            var result = controller.AddTodo(todoToAdd);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            var createdResult = result as CreatedResult;

            createdResult!.Value.Should().BeAssignableTo<TodoResponse>();

            var todo = createdResult.Value as TodoResponse;
            todo.Should().NotBeNull();
            todo.Id.Should().Be(1);
            todo.Title.Should().Be("Task 1");
            todo.Description.Should().Be("Description 1");
            todo.PercentOfComplete.Should().Be(0);
            todo.ExpirationDateTime.Should().Be(datetime);

        }

        [Fact]
        public void TodoController_GetTodoById_ReturnStatusOkWithCorrectTodo()
        {
            //Arrnage
            var mockService = new Mock<ITodoService>();
            var expirationDateTime = DateTime.Now;
            var expectedTodos = new List<TodoResponse>
            {
                new TodoResponse { Id = 1, Title = "Task 1", Description = "Description 1", PercentOfComplete = 50, ExpirationDateTime = expirationDateTime.AddDays(2) },
                new TodoResponse { Id = 2, Title = "Task 2", Description = "Description 2", PercentOfComplete = 100, ExpirationDateTime = expirationDateTime.AddDays(1) }
            };

            mockService.Setup(service => service.GetTodoById(1)).Returns(expectedTodos[0]);
            var controller = new TodoController(mockService.Object);

            //Act
            var result = controller.GetTodoById(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeAssignableTo<TodoResponse>();

            var todo = okResult.Value as TodoResponse;
            todo.Should().NotBeNull();
            todo.Id.Should().Be(1);
            todo.Title.Should().Be("Task 1");
            todo.Description.Should().Be("Description 1");
            todo.PercentOfComplete.Should().Be(50);
            todo.ExpirationDateTime.Should().Be(expirationDateTime.AddDays(2));
        }

        [Fact]
        public void TodoController_GetTodoById_ReturnStatusNotFound()
        {
            // Arrange
            var mockService = new Mock<ITodoService>();
            mockService.Setup(service => service.GetTodoById(int.MaxValue)).Returns((TodoResponse?)null);
            var controller = new TodoController(mockService.Object);

            // Act
            var result = controller.GetTodoById(int.MaxValue);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
