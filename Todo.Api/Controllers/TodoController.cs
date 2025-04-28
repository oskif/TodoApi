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
        [ProducesResponseType(200, Type = typeof(List<TodoResponse>))]
        public IActionResult GetAllTodos()
        {
            var responmse = _todoService.GetAllTodos();
            return Ok(responmse);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TodoResponse))]
        [ProducesResponseType(400)]
        public IActionResult AddTodo([FromBody] TodoAddRquest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _todoService.AddTodo(request);

            return Created($"api/todos/{response.Id}", response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(TodoResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTodoById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater then 0");
            }

            TodoResponse? response = _todoService.getTodoById(id);
            
            if (response is null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult MarkTodoAsDone(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater then 0");
            }

            bool result = _todoService.MarkTodoAsDone(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater then 0");
            }

            bool result = _todoService.DeleteTodoById(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{id}/percent")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePercentOfComplete(int id, [FromQuery(Name = "percent")] decimal percentOfComplete)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater then 0");
            }

            if (percentOfComplete < 0 && percentOfComplete > 100)
            {
                return BadRequest("PercentOfComplete must be from range 0 to 100");
            }

            bool result = _todoService.UpdatePercentOfComplete(id, percentOfComplete);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(TodoResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTodo(int id, [FromBody] TodoUpdateRequest request)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater then 0");
            }

            var response = _todoService.UpdateTodo(id, request);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (response is null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("incoming")]
        [ProducesResponseType(200, Type = typeof(List<TodoResponse>))]
        [ProducesResponseType(400)]
        public IActionResult GetIncomingTodos([FromQuery(Name = "incoming")] string incoming)
        {
            var allowedParams = new List<string>()
            {
                "today", "tomorrow", "current_week"
            };

            if (!allowedParams.Contains(incoming.ToLower()))
            {
                return BadRequest("Param incoming accepts only today, tomorrow and current_week");
            }

            var result = _todoService.GetIncomingTodos(incoming);

            return Ok(result);
        }
    }
}
