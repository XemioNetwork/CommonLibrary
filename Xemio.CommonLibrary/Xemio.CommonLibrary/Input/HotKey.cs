using System;
using System.Windows.Input;

namespace Xemio.CommonLibrary.Input
{
    /// <summary>
    /// Represents a single hot key.
    /// </summary>
    public class HotKey
    {
        #region Fields
        private bool _isEnabled;
        internal Action _hotKeyPressed;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the key.
        /// </summary>
        public Key Key { get; private set; }
        /// <summary>
        /// Gets the modifier keys.
        /// </summary>
        public ModifierKeys ModifierKeys { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="HotKey"/> is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get { return this._isEnabled; }
            set
            {
                this._isEnabled = value;
                this.OnIsEnabledChanged();
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HotKey" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="modifierKeys">The modifier keys.</param>
        /// <param name="hotKeyPressed">The hot key pressed.</param>
        public HotKey(Key key, ModifierKeys modifierKeys, Action hotKeyPressed)
        {
            this.Key = key;
            this.ModifierKeys = modifierKeys;
            this.IsEnabled = true;

            this._hotKeyPressed = hotKeyPressed;
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when <see cref="IsEnabled"/> changed.
        /// </summary>
        internal event EventHandler IsEnabledChanged;
        /// <summary>
        /// Called before the <see cref="IsEnabledChanged"/> event is called.
        /// </summary>
        protected void OnIsEnabledChanged()
        {
            if (this.IsEnabledChanged != null)
                this.IsEnabledChanged(this, EventArgs.Empty);
        }
        #endregion
    }
}