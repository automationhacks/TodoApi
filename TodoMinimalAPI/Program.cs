using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Adds database context to dependency injection (DI)
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
// Enables displaying database related exceptions
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// Adds swagger tooling
// APIExplorer provides metadata about an HTTP API, it is used by Swagger to generate Swagger documents
builder.Services.AddEndpointsApiExplorer();
// Adds Swagger OpenAPI document generator to application services
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    /**
    Enables swagger middleware for serving generated JSON doc and Swagger UI
    It is only enabled in development environment
    Exposing in production could expose sensitive details about API structure and implementation
    **/
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

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

app.Run();

static async Task<IResult> GetAllTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.ToListAsync());
}

static async Task<IResult> GetCompleteTodos(TodoDb db)
{
    // TypedResults is better than return Results as
    // It enables testability
    // Automatically returns response type metadata for OpenAPI to describe the endpoint
    // Read https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-10.0#typedresults-vs-results
    // to understand benefits of TypedResults over Results
    return TypedResults.Ok(await db.Todos.Where(todo => todo.IsComplete).ToListAsync());
}

static async Task<IResult> GetTodo(int id, TodoDb db)
{
    return await db.Todos.FindAsync(id) is Todo todo
        ? TypedResults.Ok(todo) : TypedResults.NotFound();
}

static async Task<IResult> CreateTodo(Todo todo, TodoDb db)
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/todoitems/{todo.Id}", todo);
}

static async Task<IResult> UpdateTodo(int id, Todo inputTodo, TodoDb db)
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null)
    {
        return Results.NotFound();
    }

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTodo(int id, TodoDb db)
{
    if (await db.Todos.FindAsync(id) is Todo todo)
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