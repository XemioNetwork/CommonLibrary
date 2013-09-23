using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Xemio.CommonLibrary.Security
{
    /// <summary>
    /// An implementation of <see cref="IEncrypter"/> using the <see cref="ProtectedData"/> class.
    /// </summary>
    public class WindowsEncrypter : IEncrypter
    {
        #region Implementation of IEncrypter
        /// <summary>
        /// Encrypts the given <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        public byte[] Encrypt(byte[] data)
        {
            return ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
        }
        /// <summary>
        /// Decrypts the given <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        public byte[] Decrypt(byte[] data)
        {
            return ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
        }
        #endregion
    }
}
