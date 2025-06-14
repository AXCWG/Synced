namespace Synced.Server
{
    public static class Configs
    {
        public static string DataDirectory
        {
            get
            {
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/AXCWG"))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/AXCWG");
                }
                if(!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/AXCWG" + "/SyncedServer"))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/AXCWG" + "/SyncedServer"); 
                }
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/AXCWG" + "/SyncedServer";
            }
        }
    }
}
