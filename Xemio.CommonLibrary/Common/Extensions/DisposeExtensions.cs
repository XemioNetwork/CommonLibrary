using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.CommonLibrary.Common.Extensions
{
    public static class DisposeExtensions
    {
        /// <summary>
        /// Tries to execute the <see cref="IDisposable.Dispose"/> method.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static void TryDispose(this object instance)
        {
            IDisposable disposable = instance as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }
    }
}
