using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.CommonLibrary.Security
{
    /// <summary>
    /// A default implementation of <see cref="IEncrypter"/> doing nothing.
    /// </summary>
    public class NullEncrypter : IEncrypter
    {
        #region Implementation of IEncrypter
        /// <summary>
        /// Encrypts the given <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        public byte[] Encrypt(byte[] data)
        {
            return data;
        }
        /// <summary>
        /// Decrypts the given <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        public byte[] Decrypt(byte[] data)
        {
            return data;
        }
        #endregion
    }
}
