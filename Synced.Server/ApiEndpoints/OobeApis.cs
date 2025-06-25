using Microsoft.Data.Sqlite;
using Synced.Server.Exceptions;
using System.Runtime.CompilerServices;

namespace Synced.Server.ApiEndpoints
{
    public static class OobeApis
    {
        private class SetupPayload
        {
            public required UserApis.RegisterPayload  AdminRegistration { get; set; }
            public required bool EnableRegister { get; set;  }

        }
        public static void UseOobeApis(this WebApplication app, (Configs, SqliteConnection) cfgNDb)
        {
           
                var oobeApi = app.MapGroup("/api/v1/oobe");
                oobeApi.MapGet("/", () =>
                {
                    if (Configs.isInit)
                        return Results.Ok("is oobe.");
                    else
                        return Results.BadRequest("oobe ended. "); 
                });
                oobeApi.MapPost("/setup", (HttpContext ctx, SetupPayload setupPayload) =>
                {
                    if (Configs.isInit)
                    {
                        try
                        {
                            var uuid = cfgNDb.Item2.AddUser(setupPayload.AdminRegistration.Username,
                            setupPayload.AdminRegistration.Password,
                            setupPayload.AdminRegistration.Email,
                            0);
                            cfgNDb.Item1.EnableRegister = setupPayload.EnableRegister;
                            Configs.isInit = false;
                            ctx.Session.SetString("uuid", uuid);
                            return Results.Ok();
                        }
                        catch (UserExistsException e)
                        {
                            return Results.BadRequest(e.Message);
                        }
                    }
                    else
                    {
                        return Results.BadRequest("oobe ended"); 
                    }
                });
            
            
        }
    }
}
