using Microsoft.EntityFrameworkCore;
using TodoMinimalAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    // Formats JSON
    options.SerializerOptions.WriteIndented = true;
    // Includes public fields
    options.SerializerOptions.IncludeFields = true;
});

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
    // Enables swagger middleware for serving generated JSON doc and Swagger UI
    // It is only enabled in development environment
    // Exposing in production could expose sensitive details about API structure and implementation
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.RegisterTodoItemsEndpoints();
app.Run();

