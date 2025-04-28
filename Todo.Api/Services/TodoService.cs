using Microsoft.OpenApi.Validations;
using Todo.Api.Data;
using Todo.Api.Entities;
using Todo.Api.Model.Dtos;

namespace Todo.Api.Services;

public class TodoService
{
    private readonly ApplicationDbContext _context;

    public TodoService(ApplicationDbContext context)
    {
        _context = context;
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

    public TodoResponse? getTodoById(int id)
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
            Id= todo.Id,
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

    public IEnumerable<TodoResponse> GetIncomingTodos(string incoming)
    {
        DateTime currentDate = DateTime.Now;
        List<TodoEntity> list = new();
        switch (incoming)
        {
            case "today":
                list = _context.Todos.Where(t =>
                    t.ExpirationDateTime.Date == currentDate.Date &&
                    t.ExpirationDateTime.TimeOfDay > currentDate.TimeOfDay &&
                    t.PercentOfComplete < 100).ToList();
                break;

            case "tomorrow":
                var tomorrowDate = currentDate.AddDays(1);
                list = _context.Todos.Where(t => 
                    t.ExpirationDateTime.Date == tomorrowDate.Date &&
                    t.PercentOfComplete < 100).ToList();
                break;

            case "current_week":
                int dayOfWeek = (int)currentDate.Date.DayOfWeek;

                list = _context.Todos.Where(t =>
                    (t.ExpirationDateTime.Date >= currentDate.Date && t.ExpirationDateTime.TimeOfDay > currentDate.TimeOfDay) &&
                    t.ExpirationDateTime.Date < currentDate.AddDays(7 - dayOfWeek).Date &&
                    t.PercentOfComplete < 100
                    ).ToList();
                break;

            default:
                return [];
        }
        return list.Select(item =>
            new TodoResponse()
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                ExpirationDateTime = item.ExpirationDateTime,
                PercentOfComplete = item.PercentOfComplete
            }
        );
    }
}
