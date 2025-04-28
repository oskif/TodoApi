using Todo.Api.Model.Dtos;

namespace Todo.Api.Services
{
    public interface ITodoService
    {
        IEnumerable<TodoResponse> GetAllTodos();
        TodoResponse? GetTodoById(int id);
        TodoResponse AddTodo(TodoAddRquest request);
        bool DeleteTodoById(int id);
        bool MarkTodoAsDone(int id);
        bool UpdatePercentOfComplete(int id, decimal percentOfComplete);
        TodoResponse? UpdateTodo(int id, TodoUpdateRequest request);
        IEnumerable<TodoResponse> GetIncomingTodos(string incoming);
    }
}
