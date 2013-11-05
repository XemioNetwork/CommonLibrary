using System.IO;

namespace Xemio.CommonLibrary.Storage.Files
{
    /// <summary>
    /// A file-system abstraction.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Returns whether the file exists.
        /// </summary>
        /// <param name="path">The path.</param>
        bool FileExists(string path);
        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        void DeleteFile(string path);
        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="fileMode">The file mode.</param>
        Stream OpenFile(string path, FileMode fileMode);
    }
}
