using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Synced.Server.Exceptions;

namespace Synced.Server.ApiEndpoints
{
    public static class UserApis
    {
        public class RegisterPayload
        {
            public required string Username { get; set; }
            public required string Password { get; set; }
            public string? Email { get; set; }
            public int? Role { get; set; }
        }
        public static void UseUserApi(this WebApplication app, (Configs, SqliteConnection) cfg)
        {

            var userApi = app.MapGroup("/api/v1/users");
            if (cfg.Item1.EnableRegister is not null && cfg.Item1.EnableRegister.Value)
            {
                userApi.MapPost("/register", (HttpContext ctx, [FromBody] RegisterPayload registerPayload) =>
                {
                    if (registerPayload.Role is null)
                        return Results.BadRequest("Specify a role. ");
                    try
                    {
                        var uuid = cfg.Item2.AddUser(registerPayload.Username, registerPayload.Password, registerPayload.Email, registerPayload.Role.Value);
                        ctx.Session.SetString("uuid", uuid);
                        return Results.Ok();
                    }
                    catch (UserExistsException e)
                    {
                        return Results.BadRequest(e.Message);
                    }
                });
            }
            else
            {
                Results.BadRequest("No register. ");
            }
        }
    }
}
