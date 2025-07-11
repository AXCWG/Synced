namespace Synced.Backend;

public static class Extensions
{
    public static void DirectoriesExist(params string[] directories)
    {
        var baseDir = directories[0];
        if (!Directory.Exists(baseDir))
        {
            throw new DirectoryNotFoundException("The first parameter must exist. ");
        }

        foreach (var directory in directories)
        {
            if (directory == baseDir)
            {
                continue;
            }

            try
            {
                baseDir = Path.Join(baseDir, directory);
                Directory.CreateDirectory(baseDir);
            }
            catch (IOException e)
            {
                throw new IOException("Directory failed to create: " + baseDir, e);
            }
        }
    }
}