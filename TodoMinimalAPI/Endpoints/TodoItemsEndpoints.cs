
using Microsoft.EntityFrameworkCore;

namespace TodoMinimalAPI.Endpoints;

public static class TodoItemsEndpoints
{
    public static void RegisterTodoItemsEndpoints(this WebApplication app)
    {
        var todoItems = app.MapGroup("/todoitems");
        // Get all todos
        todoItems.MapGet("/", GetAllTodos);

        // Create todo
        todoItems.MapPost("/", CreateTodo);

        // Get all completed todos
        todoItems.MapGet("/complete", GetCompleteTodos);

        // Get todo by id
        todoItems.MapGet("/{id}", GetTodo);

        // Update todo
        todoItems.MapPut("/{id}", UpdateTodo);

        // Remove a todo from in memory db
        todoItems.MapDelete("/{id}", DeleteTodo);

        // Json patch could be used to apply partial changes to a JSON
        // https://learn.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-10.0
        todoItems.MapPatch("/{id}", PartialUpdateTodo);
    }

    static async Task<IResult> GetAllTodos(TodoDb db)
    {
        return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDto(x)).ToArrayAsync());
    }

    static async Task<IResult> GetCompleteTodos(TodoDb db)
    {
        // TypedResults is better than return Results as
        // It enables testability
        // Automatically returns response type metadata for OpenAPI to describe the endpoint
        // Read https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-10.0#typedresults-vs-results
        // to understand benefits of TypedResults over Results
        return TypedResults
            .Ok(await db.Todos.Where(todo => todo.IsComplete)
                .Select(x => new TodoItemDto(x)).ToListAsync());
    }

    static async Task<IResult> GetTodo(int id, TodoDb db)
    {
        var todo = await db.Todos.FindAsync(id);
        return todo != null
            ? TypedResults.Ok(new TodoItemDto(todo))
            : TypedResults.NotFound();
    }

    static async Task<IResult> CreateTodo(TodoItemDto todoItemDto, TodoDb db)
    {
        var todoItem = new Todo
            {
                IsComplete = todoItemDto.IsComplete,
                Name = todoItemDto.Name
            }
            ;

        db.Todos.Add(todoItem);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/todoitems/{todoItemDto.Id}", todoItemDto);
    }

    static async Task<IResult> UpdateTodo(int id, TodoItemDto todoItemDto, TodoDb db)
    {
        var todo = await db.Todos.FindAsync(id);

        if (todo is null)
        {
            return TypedResults.NotFound();
        }

        todo.Name = todoItemDto.Name;
        todo.IsComplete = todoItemDto.IsComplete;

        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    static async Task<IResult> DeleteTodo(int id, TodoDb db)
    {
        // is { } todo is a null check pattern
        // it checks if result is a non null object, assign it to todo
        // { } matches any non null object
        // this code is equivalent to:
        // var todo = await db.Todos.FindAsync(id);
        // if (todo != null)
        // {
        //     ...
        // }
        if (await db.Todos.FindAsync(id) is { } todo)
        {
            db.Todos.Remove(todo);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

    static async Task<IResult> PartialUpdateTodo(int id, TodoPatchDto inputTodo, TodoDb db)
    {
        var todo = await db.Todos.FindAsync(id);

        if (todo is null) return TypedResults.NotFound();

        if (inputTodo.Name is not null) todo.Name = inputTodo.Name;
        // If you've already checked that a parameter is not null, you can use .Value
        // to get the value without getting a build error
        if (inputTodo.IsComplete is not null) todo.IsComplete = inputTodo.IsComplete.Value;

        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}