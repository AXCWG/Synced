using Microsoft.Data.Sqlite;

namespace Synced.Backend.Singletons;

public static class Runtime
{
    public static SqliteConnection Database { get; } = new($"Data Source={Path.Join(Configs.DataDir, "data.db")}");
    public static bool IsInit { get; set; } = true; 
}