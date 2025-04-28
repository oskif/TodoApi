using System;

namespace Todo.Api.Model.Dtos;

public class TodoAddRquest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required DateTime ExpirationDateTime { get; set; }
    public decimal PercentOfComplete { get; set; }
}
