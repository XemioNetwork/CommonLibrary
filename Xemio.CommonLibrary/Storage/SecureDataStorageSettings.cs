using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xemio.CommonLibrary.Security;
using Xemio.CommonLibrary.Serialization;

namespace Xemio.CommonLibrary.Storage
{
    /// <summary>
    /// Contains settings for the <see cref="SecureDataStorage"/>.
    /// </summary>
    public class SecureDataStorageSettings
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureDataStorageSettings"/> class.
        /// </summary>
        public SecureDataStorageSettings()
        {
            this.Serializer = new JsonSerializer();
            this.Encrypter = new WindowsEncrypter();

            this.KeyToFileName = f => f;
            this.TypeToDefaultKey = f => f.Name;
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
        /// Gets or sets the method used to convert a key to a file name.
        /// </summary>
        public Func<string, string> KeyToFileName { get; set; }
        /// <summary>
        /// Gets or sets the method used to convert a <see cref="Type"/> to it's default key.
        /// </summary>
        public Func<Type, string> TypeToDefaultKey { get; set; } 
        #endregion
    }
}
