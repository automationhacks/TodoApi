using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Adds database context to dependency injection (DI)
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
// Enables displaying database related exceptions
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// Get all todos
app.MapGet("/todoitems", async (TodoDb db) => await db.Todos.ToListAsync());

// Get all completed todos
app.MapGet("/todoitems/complete", async (TodoDb db) => await db.Todos.Where(todo => todo.IsComplete).ToListAsync());

// Get todo by id
app.MapGet("/todoitems/{id}",
    async (int id, TodoDb db) => await db.Todos.FindAsync(id) is Todo todo ? Results.Ok(todo) : Results.NotFound());

// Create todo
app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

// Update todo
app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null)
    {
        return Results.NotFound();
    }

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Remove a todo from in memory db
app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();
