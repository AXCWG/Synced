namespace Synced.Backend.Singletons;

public static class Configs
{
    public static string DataDir
    {
        get
        {
             Extensions.DirectoriesExist(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AXCWG", "Synced.Backend"); 
             return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AXCWG", "Synced.Backend") ; 

        }
    }
}