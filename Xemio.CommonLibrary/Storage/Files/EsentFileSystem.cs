using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Isam.Esent.Collections.Generic;

namespace Xemio.CommonLibrary.Storage.Files
{
    public class EsentFileSystem : IFileSystem, IDisposable
    {
        #region Fields
        private readonly PersistentDictionary<string, string> _dictionary;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EsentFileSystem"/> class.
        /// </summary>
        public EsentFileSystem()
            : this(Path.Combine(".", "Data"))
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EsentFileSystem"/> class.
        /// </summary>
        /// <param name="baseDirectory">The base directory.</param>
        public EsentFileSystem(string baseDirectory)
        {
            this._dictionary = new PersistentDictionary<string, string>(baseDirectory);
        #endregion
        }

        #region Implementation of IFileSystem
        /// <summary>
        /// Returns whether the file exists.
        /// </summary>
        /// <param name="path">The path.</param>
        public bool FileExists(string path)
        {
            return this._dictionary.ContainsKey(path);
        }
        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        public void DeleteFile(string path)
        {
            this._dictionary.Remove(path);
        }
        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="fileMode">The file mode.</param>
        public Stream OpenFile(string path, FileMode fileMode)
        {
            if (fileMode == FileMode.Open)
            {
                byte[] data = Convert.FromBase64String(this._dictionary[path]);
                return new MemoryStream(data);
            }
            else if (fileMode == FileMode.Create)
            {
                EsentStream stream = new EsentStream();

                stream.OnDispose = () =>
                {
                    string newData = Convert.ToBase64String(stream.ToArray());
                    this._dictionary[path] = newData;
                };

                return stream;
            }

            throw new InvalidOperationException("Invalid FileMode.");
        }
        #endregion

        #region Internal
        /// <summary>
        /// Internal stream for storing the value back into the dictionary.
        /// </summary>
        private class EsentStream : MemoryStream
        {
            public Action OnDispose { get; set; }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (disposing && this.OnDispose != null)
                    this.OnDispose();

            }
        }
        #endregion

        #region Implementation of IDisposable
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this._dictionary.Dispose();
        }
        #endregion
    }
}
