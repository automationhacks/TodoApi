// Source: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling-api?view=aspnetcore-10.0&tabs=minimal-apis#developer-exception-page
// Comment code in Program.cs and then run this app
// Go to /exception to see Developer exception page
// Otherwise keep this commented as there can only be one top level unit in the application

// var builder = WebApplication.CreateBuilder(args);
// // Generates problem details for all HTTP client and server error responses
// // that don't have body content yet
// builder.Services.AddProblemDetails();

// var app = builder.Build();

// // In production env, we should use UseExceptionHandler that provides
// // RFC 7807 compliant payload to the client
// app.UseExceptionHandler(exceptionHandlerApp =>
//     exceptionHandlerApp.Run(async context => await Results.Problem().ExecuteAsync(context)));
// // Generates a problem details response by default
// // You can specify a fallback in case IProblemDetailsService implementation is not able to
// // generate a response
// // to test this:
// //      dotnet run DeveloperExceptionPage.cs
// //      curl -i -H "Accept: application/xml" http://localhost:5152/users/-1

// app.UseStatusCodePages(statusCodeHandlerApp =>
// {
//     statusCodeHandlerApp.Run(async httpContext =>
//     {
//         var pds = httpContext.RequestServices.GetService<IProblemDetailsService>();
//         if (pds == null || !await pds.TryWriteAsync(new() { HttpContext = httpContext }))
//         {
//             // Fallback behavior
//             await httpContext.Response.WriteAsync("Fallback: An error occured");
//         }
//     });
// });

// app.MapGet("/exception", () => { throw new InvalidOperationException("Sample exception"); });
// app.MapGet("/", () => "Test by calling /exception");
// app.MapGet("/users/{id:int}", (int id) => id <= 0 ? Results.BadRequest() : Results.Ok(new User(id)));

// app.Run();

// public record User(int Id);