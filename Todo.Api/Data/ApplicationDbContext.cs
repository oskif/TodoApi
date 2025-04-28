using System;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Entities;

namespace Todo.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoEntity> Todos { get; set; }
}
