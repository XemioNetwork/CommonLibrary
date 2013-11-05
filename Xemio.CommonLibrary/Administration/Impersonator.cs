using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using Xemio.CommonLibrary.Common;

namespace Xemio.CommonLibrary.Administration
{
    /// <summary>
    /// Provides impersonation as another user.
    /// </summary>
    public class Impersonator
    {
        #region Fields
        public string Domain { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        private IntPtr _token;

        private WindowsImpersonationContext _context;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Impersonator"/> class.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public Impersonator(string domain, string username, string password)
        {
            this.Domain = domain;
            this.Username = username;
            this.Password = password;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Impersonates the current user as <see cref="Username"/>.
        /// </summary>
        public IDisposable Impersonate()
        {
            if (this._context != null)
                throw new InvalidOperationException("You can't impersonate while already impersonating. Don't nest the Impersonate calls.");

            //Just do nothing when no credentials are provided.
            //Makes it more stable because we don't throw exceptions everywhere...
            if (string.IsNullOrWhiteSpace(this.Username) ||
                string.IsNullOrWhiteSpace(this.Password) ||
                string.IsNullOrWhiteSpace(this.Domain))
            {
                return new ActionDisposer(() => { });
            }

            try
            {
                this._token = IntPtr.Zero;

                bool logonSuccessfull = LogonUser(this.Username, this.Domain, this.Password, 2, 0, ref _token);
                if (logonSuccessfull == false)
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new Win32Exception(error);
                }

                WindowsIdentity identity = new WindowsIdentity(_token);
                this._context = identity.Impersonate();

                return new ActionDisposer(this.LeaveImpersonation);
            }
            catch (Exception exception)
            {
                throw new InvalidCredentialException("The impersonation failed. For more information see the inner exception.", exception);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Leaves the impersonation.
        /// </summary>
        private void LeaveImpersonation()
        {
            this._context.Undo();

            if (this._token != IntPtr.Zero)
                CloseHandle(this._token);
        }
        #endregion

        #region Windows API
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);
        #endregion
    }
}
