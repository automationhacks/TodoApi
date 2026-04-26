// Source: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling-api?view=aspnetcore-10.0&tabs=minimal-apis#developer-exception-page
// Comment code in Program.cs and then run this app
// Go to /exception to see Developer exception page
// Otherwise keep this commented as there can only be one top level unit in the application

var builder = WebApplication.CreateBuilder(args);
// Generates problem details for all HTTP client and server error responses
// that don't have body content yet
builder.Services.AddProblemDetails();

var app = builder.Build();

// In production env, we should use UseExceptionHandler that provides
// RFC 7807 compliant payload to the client
app.UseExceptionHandler(exceptionHandlerApp =>
    exceptionHandlerApp.Run(async context => await Results.Problem().ExecuteAsync(context)));
// Generates a problem details response by default
app.UseStatusCodePages(async statusCodeContext =>
    await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
        .ExecuteAsync(statusCodeContext.HttpContext));

app.MapGet("/exception", () => { throw new InvalidOperationException("Sample exception"); });
app.MapGet("/", () => "Test by calling /exception");
app.MapGet("/users/{id:int}", (int id) => id <= 0 ? Results.BadRequest() : Results.Ok(new User(id)));

app.Run();

public record User(int Id);