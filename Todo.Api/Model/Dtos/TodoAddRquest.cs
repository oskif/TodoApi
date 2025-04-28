using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Api.Model.Dtos;

public class TodoAddRquest
{
    [Required]
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required DateTime ExpirationDateTime { get; set; }
    [Range(0, 100)]
    public decimal PercentOfComplete { get; set; }
}
