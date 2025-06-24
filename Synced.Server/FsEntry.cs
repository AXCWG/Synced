using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Synced.Server
{
    public class FsEntry
    {
        public required Microsoft.Win32.SafeHandles.SafeFileHandle FsHandle { get; init; }
        public required string Path { get; set; }
        public required DateTimeOffset LastModifiedTime { get; set; }
    }
    public class FileFsEntry : FsEntry
    {
        public required byte[] Sha256Hash { get; set; }
    }
    public class DirectoryFsEntry : FsEntry
    {
        public required List<FsEntry> Entries { get; set; }
    }

}