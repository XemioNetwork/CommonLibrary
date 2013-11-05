using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.IO.IsolatedStorage;

namespace Xemio.CommonLibrary.Storage
{
    /// <summary>
    /// A secure data storage with exchangable serialization and encryption mechanisms.
    /// </summary>
    public class DataStorage : IDisposable, IDataStorage
    {
        #region Properties
        /// <summary>
        /// Gets the settings.
        /// </summary>
        public DataStorageSettings Settings { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DataStorage"/> class.
        /// </summary>
        public DataStorage()
            : this(new DataStorageSettings())
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataStorage"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public DataStorage(DataStorageSettings settings)
        {
            this.Settings = settings;
        }
        #endregion

        #region Implementation of IDataStorage
        /// <summary>
        /// Stores the given <paramref name="instance"/> using a default key.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instance">The instance.</param>
        public void Store<T>(T instance)
        {
            string key = this.Settings.TypeToDefaultKey(typeof(T));
            this.Store(instance, key);
        }
        /// <summary>
        /// Stores the given <paramref name="instance"/> using the given <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        public void Store<T>(T instance, string key)
        {
            string fileName = this.Settings.KeyToFileName(key);

            if (this.Settings.FileSystem.FileExists(fileName))
            {
                this.Settings.FileSystem.DeleteFile(fileName);
            }

            byte[] serializedInstance = this.Serialize(instance);
            byte[] protectedInstance = this.Settings.Encrypter.Encrypt(serializedInstance);

            using (Stream stream = this.Settings.FileSystem.OpenFile(fileName, FileMode.Create))
            {
                stream.Write(protectedInstance, 0, protectedInstance.Length);
            }
        }
        /// <summary>
        /// Retrieves the instance using a default key.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        public T Retrieve<T>()
        {
            string key = this.Settings.TypeToDefaultKey(typeof(T));
            return this.Retrieve<T>(key);
        }
        /// <summary>
        /// Retrieves the instance using the given <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="key">The key.</param>
        public T Retrieve<T>(string key)
        {
            string fileName = this.Settings.KeyToFileName(key);

            if (this.Settings.FileSystem.FileExists(fileName) == false)
            {
                return default(T);
            }

            byte[] protectedData;
            using (Stream stream = this.Settings.FileSystem.OpenFile(fileName, FileMode.Open))
            {
                protectedData = new byte[stream.Length];
                stream.Read(protectedData, 0, (int)stream.Length);
            }

            byte[] serializedInstance = this.Settings.Encrypter.Decrypt(protectedData);
            return this.Deserialize<T>(serializedInstance);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Serializes the specified instance into an byte array.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instance">The instance.</param>
        private byte[] Serialize<T>(T instance)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                this.Settings.Serializer.Serialize(instance, stream);

                return stream.ToArray();
            }
        }
        /// <summary>
        /// Deserializes the speicied byte array into an instance of the given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="data">The data.</param>
        private T Deserialize<T>(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                try
                {
                    return (T)this.Settings.Serializer.Deserialize(typeof(T), stream);
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
        }
        #endregion

        #region Implementation of IDisposable
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Settings.Dispose();
        }
        #endregion
    }
}
