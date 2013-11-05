using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.CommonLibrary.Storage.Files
{
    /// <summary>
    /// A <see cref="IFileSystem"/> using an <see cref="IsolatedStorageFile"/>.
    /// </summary>
    public class IsolatedFileSystem : IFileSystem, IDisposable
    {
        #region Fields
        private readonly IsolatedStorageFile _isolatedStorageFile;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedFileSystem"/> class.
        /// </summary>
        public IsolatedFileSystem()
        {
            this._isolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly();
        }
        #endregion

        #region Implementation of IFileSystem
        /// <summary>
        /// Returns whether the file exists.
        /// </summary>
        /// <param name="path">The path.</param>
        public bool FileExists(string path)
        {
            return this._isolatedStorageFile.FileExists(path);
        }
        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        public void DeleteFile(string path)
        {
            this._isolatedStorageFile.DeleteFile(path);
        }
        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="fileMode">The file mode.</param>
        public Stream OpenFile(string path, FileMode fileMode)
        {
            return this._isolatedStorageFile.OpenFile(path, fileMode);
        }
        #endregion

        #region Implementation of IDisposable
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this._isolatedStorageFile.Dispose();
        }
        #endregion
    }
}
