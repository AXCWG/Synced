namespace Synced.Server.ApiEndpoints
{
    public static class UserApis
    {
        public static void UseUserApi(this WebApplication app, Configs cfg)
        {
            var userApi = app.MapGroup("/api/v1/users");
            if (cfg.EnableRegister is not null && cfg.EnableRegister.Value)
            {
                userApi.MapPost("/register", (HttpContext ctx) =>
                {

                });
            }
        }
    }
}
