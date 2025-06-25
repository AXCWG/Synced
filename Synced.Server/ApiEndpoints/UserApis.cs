using Microsoft.Data.Sqlite;

namespace Synced.Server.ApiEndpoints
{
    public static class UserApis
    {
        private class RegisterPayload
        {
            public required string Username { get; set; }
            public required string Password { get; set; }
            public string? Email { get; set; }
        }
        public static void UseUserApi(this WebApplication app, (Configs, SqliteConnection) cfg)
        {
            
            var userApi = app.MapGroup("/api/v1/users");
            if (cfg.Item1.EnableRegister is not null && cfg.Item1.EnableRegister.Value)
            {
                userApi.MapPost("/register", (HttpContext ctx, RegisterPayload registerPayload) =>
                {
                    cfg.Item2.CheckUsername(registerPayload.Username, out var rows);
                    if (rows.Count == 0)
                    {
                        cfg.Item2.AddUser(registerPayload.Username, registerPayload.Password,  registerPayload.Email, Configs.isInit ? 0 : 1);
                    }
                });
            }
        }
    }
}
