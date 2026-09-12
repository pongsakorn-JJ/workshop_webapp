using System.IO.Pipelines;
using TodoApi.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var todos = new List <TodoGetDto>
{
    new(1, "Learn c", true),
    new(2, "Learn .net", false),
    new(3, "Learn web", false)
};

app.MapGet("/api/todos", () => Results.Ok(todos));

app.MapGet("/api/todos/{id}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);

    return todo is not null ? Results.Ok(todo) : Results.NotFound();
});

app.MapPost("/api/todos", (TodoPostDto todoPostDto) =>
{
    var nextId = todos.Count == 0 ? 1 : todos.Max(t => t.Id) + 1;

    var todo = new TodoGetDto(nextId, todoPostDto.Title, false);
    todos.Add(todo);

    return Results.Created($"/api/todos/{todo.Id}", todo);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
