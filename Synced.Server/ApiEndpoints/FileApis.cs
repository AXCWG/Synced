namespace Synced.Server.ApiEndpoints
{
    public static class FileApis
    {
        public static void UseFileApis(this WebApplication app, Configs cfg)
        {
            var fileApi = app.MapGroup("/api/v1/files");
            
        }
    }
}
