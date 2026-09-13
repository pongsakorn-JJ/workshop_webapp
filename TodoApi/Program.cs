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

var todoGroup = app.MapGroup("/api/todos").WithTags("Todos");

#region In-memory Endpoint

// var todos = new List<TodoGetDto>
// {
//     new TodoGetDto(1, "Learn C#", true),
//     new TodoGetDto(2, "Learn ASP.NET Core", false),
//     new TodoGetDto(3, "Build a web API", false)
// };

// todoGroup.MapGet("/", () => Results.Ok(todos));

// todoGroup.MapGet("/{id}", (int id) =>
// {
//     var todo = todos.FirstOrDefault(t => t.Id == id);

//     return todo is not null ? Results.Ok(todo) : Results.NotFound();

// });

//     todoGroup.MapPost("/", (TodoPostDto dto) =>
//     {
//         var nextId = todos.Count == 0 ? 1 : todos.Max(t => t.Id) + 1;

//         var todo = new TodoGetDto(nextId, dto.Title, false);
//         todos.Add(todo);

//         return Results.Created($"/api/todos/{todo.Id}", todo);
//     });

// todoGroup.MapPut("/{id}", (int id, TodoPutDto dto) =>
// {
//     try
//     {
//         var index = todos.FindIndex(t => t.Id == id);
//         if (index == -1) return Results.NotFound();

//         todos[index] = todos[index] with
//         {
//             Title = dto.Title,
//             IsCompleted = dto.IsCompleted 
//         };

//         return Results.Ok(todos[index]);
//     }
//     catch (Exception ex)
//     {
//         return Results.Problem(ex.Message);
//     }
// });

// todoGroup.MapDelete("/{id}", (int id) =>
// {
//     try
//     {
//         var todo = todos.FirstOrDefault(t => t.Id == id);
//         if (todo is null) return Results.NotFound();

//         todos.Remove(todo);
//         return Results.NoContent();
//     }
//     catch(ArgumentNullException ex)
//     {
//         return Results.Problem($"Parameter is null: {ex.ParamName}");
//     }
//     catch (Exception ex)
//     {
//         return Results.Problem(ex.Message);
//     }

// });

#endregion

#region Database Endpoint

todoGroup.MapGet("/", async (TodoDbContext db) =>
{
    var todos = await db.Todos.ToListAsync();

    return todos.Count == 0 ? Results.NotFound() : Results.Ok(todos);
});

todoPostGroup.MapPost("/", async (AppDbContext db, TodoDbContext dto) =>
{
    // read data from the database
    var lastTodo = await db.Todos.OrderByDescending(t => t.Id).FirstOrDefaultAsync();
    var nextId = lastTodo is null ? 1 : lastTodo.Id + 1;

    var todos = new TodoGetDto
    {
        Id = nextId,
        Title = dto.Title,
        IsCompleted = False,
        CreatedAt = DateTime.UtcNow

    };

    db.Todos.Add(todos);
    await db.SaveChangesAsync();

    var tosoGetDto = new TodoGetDto(todos.Id,todos.Title,todos.IsCompleted,todos.CreatedAt);

    return Results.Created($"/api/todos/{todos.Id}", todos);

});

#endregion

app.Run();