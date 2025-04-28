using System;

namespace Todo.Api.Model.Dtos;

public class TodoUpdateRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? ExpirationDateTime { get; set; }
    public decimal? PercentOfComplete { get; set; }
}
