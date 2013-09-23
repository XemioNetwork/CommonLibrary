using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using System.Windows.Interop;

namespace Xemio.CommonLibrary.Input
{
    public class HotKeyManager : IDisposable
    {
        #region Constants
        /// <summary>
        /// The message kind of a <see cref="HotKey"/>.
        /// </summary>
        private const int HotKeyMessageKind = 786;
        /// <summary>
        /// The error number when the <see cref="HotKey"/> is already registered.
        /// </summary>
        private const int HotKeyAlreadyRegisteredError = 1409;
        #endregion

        #region Windows Interop
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int RegisterHotKey(IntPtr hwnd, int id, int modifiers, int key);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int UnregisterHotKey(IntPtr hwnd, int id);
        #endregion

        #region Fields
        private readonly List<HotKey> _hotKeys = new List<HotKey>();
        private readonly HwndSource _hwndSource;
        #endregion

        #region Properties
        /// <summary>
        /// Gets all <see cref="HotKey"/>s currently registered.
        /// </summary>
        public ReadOnlyCollection<HotKey> HotKeys
        {
            get { return new ReadOnlyCollection<HotKey>(this._hotKeys); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HotKeyManager"/> class.
        /// </summary>
        public HotKeyManager(HwndSource hwndSource)
        {
            this._hwndSource = hwndSource;
            this._hwndSource.AddHook(this.HwndHandler);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds the given <see cref="HotKey"/>.
        /// </summary>
        /// <param name="hotKey">The hot key.</param>
        public void AddHotKey(HotKey hotKey)
        {
            if (this._hotKeys.Contains(hotKey))
                return;

            this._hotKeys.Add(hotKey);

            hotKey.IsEnabledChanged += this.HotKeyOnIsEnabledChanged;

            if (hotKey.IsEnabled)
                this.RegisterHotKey(hotKey);
        }
        /// <summary>
        /// Removes the given <see cref="HotKey"/>.
        /// </summary>
        /// <param name="hotKey">The hot key.</param>
        public void RemoveHotKey(HotKey hotKey)
        {
            if (this._hotKeys.Contains(hotKey))
            { 
                this.UnRegisterHotKey(hotKey);
                this._hotKeys.Remove(hotKey);

                hotKey.IsEnabledChanged -= this.HotKeyOnIsEnabledChanged;
            }
        }
        /// <summary>
        /// Removes all hot keys.
        /// </summary>
        public void RemoveAllHotKeys()
        {
            foreach (HotKey hotKey in this._hotKeys)
            {
                this.UnRegisterHotKey(hotKey);
                hotKey.IsEnabledChanged -= this.HotKeyOnIsEnabledChanged;
            }

            this._hotKeys.Clear();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Called when the <see cref="HotKey.IsEnabled"/> property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void HotKeyOnIsEnabledChanged(object sender, EventArgs eventArgs)
        {
            HotKey hotKey = (HotKey)sender;

            if (hotKey.IsEnabled)
            {
                this.RegisterHotKey(hotKey);
            }
            else
            {
                this.UnRegisterHotKey(hotKey);
            }
        }
        /// <summary>
        /// Registers the <see cref="HotKey"/> at windows.
        /// </summary>
        /// <param name="hotKey">The hot key.</param>
        private void RegisterHotKey(HotKey hotKey)
        {
            if ((int)this._hwndSource.Handle == 0)
                throw new InvalidOperationException("The handle is invalid.");

            RegisterHotKey(this._hwndSource.Handle, this._hotKeys.IndexOf(hotKey), (int) hotKey.ModifierKeys, KeyInterop.VirtualKeyFromKey(hotKey.Key));

            int possibleError = Marshal.GetLastWin32Error();
            if (possibleError != 0 && possibleError != HotKeyAlreadyRegisteredError)
            {
                throw new Win32Exception(possibleError);
            }
        }
        /// <summary>
        /// Unregisters the <see cref="HotKey"/> from windows.
        /// </summary>
        /// <param name="hotKey">The hot key.</param>
        private void UnRegisterHotKey(HotKey hotKey)
        {
            if ((int)this._hwndSource.Handle == 0 && hotKey.IsEnabled)
                return;
            
            UnregisterHotKey(this._hwndSource.Handle, this._hotKeys.IndexOf(hotKey));

            int possibleError = Marshal.GetLastWin32Error();
            if (possibleError != 0)
                throw new Win32Exception(possibleError);
        }
        /// <summary>
        /// Called by windows.
        /// </summary>
        private IntPtr HwndHandler(IntPtr hwnd, int messageKind, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (messageKind == HotKeyMessageKind)
            {
                int hotKeyIndex = (int) wParam;
                if (hotKeyIndex < this._hotKeys.Count)
                {
                    HotKey hotKey = this._hotKeys[hotKeyIndex];
                    hotKey._hotKeyPressed();
                }
            }

            return new IntPtr(0);
        }
        #endregion

        #region Implementation of IDisposable
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.RemoveAllHotKeys();

            this._hwndSource.RemoveHook(this.HwndHandler);
        }
        #endregion
    }
}
