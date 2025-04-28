using Microsoft.AspNetCore.Mvc;
using Todo.Api.Model.Dtos;
using Todo.Api.Services;

namespace Todo.Api.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoService _todoService;
        public TodoController(TodoService todoService)
        {
            _todoService = todoService;
        }
        [HttpGet]
        public IActionResult GetAllTodos()
        {
            return Ok(_todoService.GetAllTodos());
        }

        [HttpPost]
        public IActionResult AddTodo([FromBody] TodoAddRquest request)
        {
            _todoService.AddTodo(request);
            return Created();
        }

        [HttpGet("{id}")]
        public IActionResult GetTodoById(int id)
        {
            TodoResponse? response = _todoService.getTodoById(id);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public IActionResult MarkTodoAsDone(int id)
        {
            bool result = _todoService.MarkTodoAsDone(id);

            if (result)
            {
                return Ok();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            bool result = _todoService.DeleteTodoById(id);

            if (result)
            {
                return Ok();
            }

            return NoContent();
        }

        [HttpPatch("{id}/percent")]
        public IActionResult UpdatePercentOfComplete(int id, [FromQuery(Name = "percent")] decimal percentOfComplete)
        {
            bool result = _todoService.UpdatePercentOfComplete(id, percentOfComplete);

            if (result)
            {
                return Ok();
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTodo(int id, [FromBody] TodoUpdateRequest request)
        {
            var response = _todoService.UpdateTodo(id, request);

            if (response is null)
            {
                return NoContent();
            }

            return Ok(response);
        }

        [HttpGet("incoming")]
        public IActionResult GetIncomingTodos([FromQuery(Name = "incoming")] string incoming)
        {
            var allowedParams = new List<string>()
            {
                "today", "tomorrow", "curent_week"
            };

            if (!allowedParams.Contains(incoming.ToLower()))
            {
                return BadRequest();
            }

            return Ok(incoming);
        }
    }
}
