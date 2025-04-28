using Todo.Api.Data;
using Todo.Api.Entities;
using Todo.Api.Model.Dtos;

namespace Todo.Api.Services;
public class TodoService : ITodoService
{
    private readonly ApplicationDbContext _context;

    public TodoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public TodoResponse AddTodo(TodoAddRquest request)
    {
        TodoEntity todo = new()
        {
            Title = request.Title,
            Description = request.Description,
            ExpirationDateTime = request.ExpirationDateTime,
            PercentOfComplete = request.PercentOfComplete
        };

        _context.Todos.Add(todo);
        _context.SaveChanges();

        return new()
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            PercentOfComplete = todo.PercentOfComplete,
            ExpirationDateTime = todo.ExpirationDateTime
        };
    }

    public bool DeleteTodoById(int id)
    {
        var todo = _context.Todos.Where(t => t.Id.Equals(id)).First();

        if (todo is null)
        {
            return false;
        }

        _context.Todos.Remove(todo);
        _context.SaveChanges();

        return true;
    }

    public IEnumerable<TodoResponse> GetAllTodos()
    {
        var todos = _context.Todos.ToList();

        return todos.Select(todo =>
            new TodoResponse
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                ExpirationDateTime = todo.ExpirationDateTime,
                PercentOfComplete = todo.PercentOfComplete
            }
        );
    }

    public IEnumerable<TodoResponse> GetIncomingTodos(string incoming)
    {
        throw new NotImplementedException();
    }

    public TodoResponse? GetTodoById(int id)
    {
        var todo = _context.Todos.Where(t => t.Id.Equals(id)).FirstOrDefault();

        if (todo is null)
        {
            return null;
        }

        return new()
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            ExpirationDateTime = todo.ExpirationDateTime,
            PercentOfComplete = todo.PercentOfComplete
        };
    }

    public bool MarkTodoAsDone(int id)
    {
        var todo = _context.Todos.Where(t => t.Id.Equals(id)).First();

        if (todo is null)
        {
            return false;
        }

        todo.PercentOfComplete = 100;
        _context.SaveChanges();

        return true;
    }


    public TodoResponse? UpdateTodo(int id, TodoUpdateRequest request)
    {
        var todo = _context.Todos.Where(t => t.Id.Equals(id)).First();

        if (todo is null)
        {
            return null;
        }

        todo.Title = request.Title ?? todo.Title;
        todo.Description = request.Description ?? todo.Description;
        todo.PercentOfComplete = request.PercentOfComplete ?? todo.PercentOfComplete;
        todo.ExpirationDateTime = request.ExpirationDateTime ?? todo.ExpirationDateTime;

        _context.SaveChanges();

        return new()
        {
            Title = todo.Title,
            Description = todo.Description,
            PercentOfComplete = todo.PercentOfComplete,
            ExpirationDateTime = todo.ExpirationDateTime
        };
    }

    public bool UpdatePercentOfComplete(int id, decimal percentOfComplete)
    {
        var todo = _context.Todos.Where(t => t.Id.Equals(id)).First();

        if (todo is null)
        {
            return false;
        }

        todo.PercentOfComplete = percentOfComplete;
        _context.SaveChanges();

        return true;
    }
}
