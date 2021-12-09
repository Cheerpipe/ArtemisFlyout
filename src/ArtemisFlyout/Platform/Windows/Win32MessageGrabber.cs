using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ArtemisFlyout.Platform.Windows
{
    public class Win32MessageGrabber : NativeWindow, IDisposable
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);

        public event EventHandler<Message> MessageReceived;
        public Win32MessageGrabber()
        {
            // create the handle for the window.
            CreateHandle(new CreateParams());
        }

        /// <summary>
        /// Overridden to get the notifications.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            MessageReceived?.Invoke(this, m);
        }

        #region IDisposable Members

        public void Dispose()
        {
            DestroyHandle();
        }

        #endregion
    }
}
