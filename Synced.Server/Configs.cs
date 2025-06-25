using System.Text.Json;
using System.Text.Json.Serialization;

namespace Synced.Server
{
    [JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
    [JsonSerializable(typeof(Configs))]
    public partial class AppSerializerContext : JsonSerializerContext
    {
    }
    public class Configs
    {
        public static string DataDirectory
        {
            get
            {
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/AXCWG"))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/AXCWG");
                }
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/AXCWG" + "/SyncedServer"))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/AXCWG" + "/SyncedServer");
                }
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/AXCWG" + "/SyncedServer";
            }

        }
        public bool? EnableRegister
        {
            get; set;
        }

        public static bool isInit
        {
            get;
            set;
        }
        
    }
}
