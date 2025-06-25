using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.FileProviders;
using Synced.Server;
using Synced.Server.ApiEndpoints;
using System.Text.Json.Serialization;


var cfg = new Func<Configs>(() =>
{
    if (File.Exists($"{Configs.DataDirectory}/Config.json"))
    {
        return JsonSerializer.Deserialize(File.ReadAllText($"{Configs.DataDirectory}/Config.json"), AppSerializerContext.Default.Configs) ?? new Configs();
    }
    else
    {
        return new Configs();
    }
})();

Console.CancelKeyPress += (_, _) =>
{
    File.WriteAllText($"{Configs.DataDirectory}/Config.json", JsonSerializer.Serialize(cfg, AppSerializerContext.Default.Configs)); ;
};
var connection = new SqliteConnection($"Data Source={Configs.DataDirectory}/App.db");
connection.Open();
connection.DbInitialize();

Configs.isInit = false; 
if (!File.Exists(Configs.DataDirectory + "/SETUP"))
{
    File.Create(Configs.DataDirectory + "/SETUP").Close();
    Configs.isInit = true;
    Console.WriteLine("First launch detected: Entering setup mode. ");
}



var builder = WebApplication.CreateBuilder(args);

#region Services
builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromDays(7);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
    o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; 
});
builder.Services.AddAuthorization(); 
builder.Services.AddCors(option =>
{
    option.AddPolicy(name: "all", policy =>
    {
        policy.WithOrigins("localhost:5261").WithHeaders("Content-Type").AllowCredentials(); 
    });
});
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppSerializerContext.Default);
});
#endregion

var app = builder.Build();
app.UseCors(); 
app.UseAuthorization();
app.UseSession(); 



app.MapGet("/", () =>
{
    if (Configs.isInit)
    {
        return Results.Text("Please interact over with a client. ");
    }
    return Results.NotFound();
});

app.UseUserApi((cfg, connection));
app.UseOobeApis((cfg, connection));
app.UseFileApis(cfg); 

//todosApi.MapGet("/", () => sampleTodos);
//todosApi.MapGet("/{id}", (int id) =>
//    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
//        ? Results.Ok(todo)
//        : Results.NotFound());

app.Run();


