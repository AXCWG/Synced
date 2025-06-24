using Microsoft.Data.Sqlite;
using Microsoft.Extensions.FileProviders;
using Synced.Server;
using Synced.Server.ApiEndpoints;
using System.Text.Json.Serialization;

var cfg = new Configs();
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
        return Results.Text("Please interact over with a client. ");
    }
    return Results.NotFound();
});

app.UseUserApi(cfg);
app.UseFileApis(cfg); 





//todosApi.MapGet("/", () => sampleTodos);
//todosApi.MapGet("/{id}", (int id) =>
//    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
//        ? Results.Ok(todo)
//        : Results.NotFound());

app.Run();


