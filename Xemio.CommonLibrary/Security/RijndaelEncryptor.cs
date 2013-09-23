using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Xemio.CommonLibrary.Security
{
    /// <summary>
    /// An implementation of <see cref="IEncrypter"/> using a <see cref="RijndaelManaged"/>.
    /// </summary>
    public class RijndaelEncryptor : IEncrypter
    {
        #region Fields
        private readonly RijndaelManaged _rijndael;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RijndaelEncryptor"/> class.
        /// </summary>
        /// <param name="password">The password.</param>
        public RijndaelEncryptor(string password)
        {
            Rfc2898DeriveBytes passwordDeriveBytes = new Rfc2898DeriveBytes(password, Encoding.Default.GetBytes(password));

            this._rijndael = new RijndaelManaged
                                 {
                                     Key = passwordDeriveBytes.GetBytes(32),
                                     IV = passwordDeriveBytes.GetBytes(16)
                                 };
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RijndaelEncryptor"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="iv">The iv.</param>
        public RijndaelEncryptor(byte[] key, byte[] iv)
        {
            this._rijndael = new RijndaelManaged
                                 {
                                     Key = key, 
                                     IV = iv
                                 };
        }
        #endregion

        #region Implementation of IEncrypter
        /// <summary>
        /// Encrypts the given <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        public byte[] Encrypt(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(stream, this._rijndael.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                }

                return stream.ToArray();
            }
        }
        /// <summary>
        /// Decrypts the given <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        public byte[] Decrypt(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(stream, this._rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                }

                return stream.ToArray();
            }
        }
        #endregion
    }
}
