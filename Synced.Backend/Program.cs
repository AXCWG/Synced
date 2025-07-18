using Synced.Backend.Singletons;

namespace Synced.Backend;

public static class Program
{
    public static void Main(string[] args)
    {
        if (File.Exists(Path.Join(Configs.DataDir, "INIT")))
        {
            Runtime.IsInit = false; 
        }
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.
        
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        
        var app = builder.Build();
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseHttpsRedirection();
        
        app.UseAuthorization();
        
        
        app.MapControllers();
        
        
        app.Run();
    }
}