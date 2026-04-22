using Microsoft.EntityFrameworkCore;

// Defines db context, this co-ordinates EntityFramework functionality for a data model
class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options) : base(options)
    {
    }

    public DbSet<Todo> Todos => Set<Todo>();
}