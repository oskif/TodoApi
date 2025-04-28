using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Api.Entities;

[Table("todo")]
public class TodoEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public required string Title { get; set; }

    [Column("description")]
    public required string Description { get; set; }

    [Column("expiration_date_time")]
    public required DateTime ExpirationDateTime { get; set; }

    [Column("percent_of_complete")]
    [Range(0, 100)]
    public decimal PercentOfComplete { get; set; }
}
