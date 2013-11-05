using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xemio.CommonLibrary.Common
{
    /// <summary>
    /// Executes the <see cref="DisposeAction"/> on disposing.
    /// </summary>
    public class ActionDisposer : IDisposable
    {
        #region Properties
        /// <summary>
        /// Gets the dispose action.
        /// </summary>
        public Action DisposeAction { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionDisposer"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public ActionDisposer(Action action)
        {
            this.DisposeAction = action;
        }
        #endregion

        #region Implementation of IDisposable
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.DisposeAction();
        }
        #endregion
    }
}
