using System.IO;

namespace Xemio.CommonLibrary.Storage.Files
{
    /// <summary>
    /// A <see cref="IFileSystem"/> implementation using the default file-system.
    /// </summary>
    public class FileSystem : IFileSystem
    {
        #region Properties
        /// <summary>
        /// Gets the base path.
        /// </summary>
        public string BasePath { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystem"/> class.
        /// </summary>
        public FileSystem()
            : this(Path.Combine(".", "Data"))
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystem"/> class.
        /// </summary>
        /// <param name="basePath">The base path.</param>
        public FileSystem(string basePath)
        {
            this.BasePath = basePath;
        }
        #endregion

        #region Implementation of IFileSystem
        /// <summary>
        /// Returns whether the file exists.
        /// </summary>
        /// <param name="path">The path.</param>
        public bool FileExists(string path)
        {
            return File.Exists(this.GetFullPath(path));
        }
        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        public void DeleteFile(string path)
        {
            File.Delete(this.GetFullPath(path));
        }
        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="fileMode">The file mode.</param>
        public Stream OpenFile(string path, FileMode fileMode)
        {
            string fullPath = this.GetFullPath(path);

            string directory = Path.GetDirectoryName(fullPath);
            if (Directory.Exists(directory) == false)
                Directory.CreateDirectory(directory);

            return File.Open(fullPath, fileMode);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <param name="part">The part.</param>
        private string GetFullPath(string part)
        {
            return Path.Combine(this.BasePath, part);
        }
        #endregion
    }
}
