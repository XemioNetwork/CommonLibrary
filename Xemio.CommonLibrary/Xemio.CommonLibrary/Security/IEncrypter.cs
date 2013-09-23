using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xemio.CommonLibrary.Security
{
    /// <summary>
    /// Abstracted data encryption.
    /// </summary>
    public interface IEncrypter
    {
        /// <summary>
        /// Encrypts the given <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        byte[] Encrypt(byte[] data);
        /// <summary>
        /// Decrypts the given <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        byte[] Decrypt(byte[] data);
    }
}
