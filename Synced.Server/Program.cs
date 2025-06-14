using Microsoft.Data.Sqlite;
using Microsoft.Extensions.FileProviders;
using Synced.Server;
using System.Text.Json.Serialization;

var connection = new SqliteConnection($"Data Source={Configs.DataDirectory}/App.db");
connection.Open();
connection.DbInitialize();

bool init = false;
if (!File.Exists(Configs.DataDirectory + "/SETUP"))
{
    File.Create(Configs.DataDirectory + "/SETUP").Close();
    init = true;
}



var builder = WebApplication.CreateSlimBuilder(args);

//builder.Services.ConfigureHttpJsonOptions(options =>
//{
//    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
//});

var app = builder.Build();


//var sampleTodos = new Todo[] {
//    new(1, "Walk the dog"),
//    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
//    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
//    new(4, "Clean the bathroom"),
//    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
//};

app.MapGet("/", () =>
{
    if (init)
    {
        return Results.Text("Please setup on a client. ");
    }
    return Results.NotFound();
});

var todosApi = app.MapGroup("/todos");
//todosApi.MapGet("/", () => sampleTodos);
//todosApi.MapGet("/{id}", (int id) =>
//    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
//        ? Results.Ok(todo)
//        : Results.NotFound());

app.Run();


