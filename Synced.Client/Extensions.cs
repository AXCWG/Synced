using System.IO;

namespace Synced.Client.Windows
{
    internal static class Extensions
    {
        public static bool FsExists(this string path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }
        public static string FsReadAllText(this string path)
        {
            if (!path.FsExists()) throw new FileNotFoundException($"File or directory '{path}' does not exist.");
            return File.ReadAllText(path);
        }
        public static void FsWriteAllText(this string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}
