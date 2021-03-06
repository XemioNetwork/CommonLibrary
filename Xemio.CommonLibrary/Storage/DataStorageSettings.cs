﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xemio.CommonLibrary.Common.Extensions;
using Xemio.CommonLibrary.Security;
using Xemio.CommonLibrary.Serialization;
using Xemio.CommonLibrary.Storage.Files;

namespace Xemio.CommonLibrary.Storage
{
    /// <summary>
    /// Contains settings for the <see cref="DataStorage"/>.
    /// </summary>
    public class DataStorageSettings : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DataStorageSettings"/> class.
        /// </summary>
        public DataStorageSettings()
        {
            this.Serializer = new JsonSerializer();
            this.Encrypter = new NullEncrypter();
            this.FileSystem = new FileSystem();

            this.KeyToFileName = f => f;
            this.TypeToDefaultKey = f => f.ToString();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the serializer.
        /// </summary>
        public ISerializer Serializer { get; set; }
        /// <summary>
        /// Gets or sets the encrypter.
        /// </summary>
        public IEncrypter Encrypter { get; set; }
        /// <summary>
        /// Gets or sets the file system.
        /// </summary>
        public IFileSystem FileSystem { get; set; }
        /// <summary>
        /// Gets or sets the method used to convert a key to a file name.
        /// </summary>
        public Func<string, string> KeyToFileName { get; set; }
        /// <summary>
        /// Gets or sets the method used to convert a <see cref="Type"/> to it's default key.
        /// </summary>
        public Func<Type, string> TypeToDefaultKey { get; set; }
        #endregion

        #region Implementation of IDisposable
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Serializer.TryDispose();
            this.Encrypter.TryDispose();
            this.FileSystem.TryDispose();
        }
        #endregion
    }
}
