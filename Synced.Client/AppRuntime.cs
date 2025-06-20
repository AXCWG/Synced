using System.IO;
using System.Text.Json;

namespace Synced.Client.Windows
{
    public class AppRuntime
    {
        private Task ConfigSaving { get; set; }
        public AppRuntime()
        {

            AppConfig = new Func<Config>(() =>
            {
                try
                {
                    return JsonSerializer.Deserialize<Config>(ConfigFilePath.FsReadAllText(), JsonSerializerOptions.Default) ?? new Config();
                }
                catch (JsonException)
                {
                    return new Config();
                }

            })();
            ConfigSaving = new Task(async () =>
            {
                while (true)
                {
                    ConfigFilePath.FsWriteAllText(JsonSerializer.Serialize(AppConfig, JsonSerializerOptions.Default));
                    await Task.Delay(3000);
                }
            });
            ConfigSaving.Start();
        }
        public Config AppConfig { get; set; }
        private string DataDirectory
        {
            get
            {
                if (!(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/AXCWG").FsExists())
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/AXCWG");
                }
                if (!(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/AXCWG/SyncedClient").FsExists())
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/AXCWG/SyncedClient");
                }
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/AXCWG/SyncedClient";
            }
        }
        public string ConfigFilePath
        {
            get
            {
                if (!(DataDirectory + "/config.json").FsExists())
                {
                    File.WriteAllText(DataDirectory + "/config.json", "{}");
                }
                return DataDirectory + "/config.json";

            }
        }
    }
    public class Config
    {
        public class Node
        {
            public Uri? ServerUri { get; set; }
            public string? Username { get; set; }
            public string? Password { get; set; }

        }
        public List<Node> Nodes { get; set; } = new();
        public bool Init { get; set; } = false;

    }
}
