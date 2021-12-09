using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using AvaloniaTrayIcon = Avalonia.Controls.TrayIcon;
using Timer = System.Timers.Timer;

namespace ArtemisFlyout.Services
{
    public class TrayIconService : ITrayIconService, IDisposable
    {
        private readonly IFlyoutService _flyoutService;
        private readonly AvaloniaTrayIcon _trayIcon;

        public TrayIconService(IFlyoutService flyoutService)
        {
            _flyoutService = flyoutService;
            _flyoutService.PreLoad();
            _trayIcon = new AvaloniaTrayIcon();
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var icon = new WindowIcon(assets.Open(new Uri(@"resm:ArtemisFlyout.Assets.Flyout.ico")));
            _trayIcon.Icon = icon;
            _trayIcon.Clicked += TrayIcon_Clicked;
            _win32MessageGrabber.TaskbarCreated += _window_TaskbarCreated;
        }

        private void _window_TaskbarCreated(object sender, EventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            _trayIcon.IsVisible = false;
            _trayIcon.IsVisible = true;
        }

        public void Show()
        {
            _trayIcon.Menu = new NativeMenu();
            NativeMenuItem exitMenu = new("Exit Artemis Flyout");
            exitMenu.Click += ExitMenu_Click;
            _trayIcon.Menu.Items.Add(exitMenu);
            _trayIcon.IsVisible = true;
        }
        public void Hide()
        {
            _trayIcon.IsVisible = false;
        }

        private void ExitMenu_Click(object sender, EventArgs e)
        {
            Program.RunCancellationTokenSource.Cancel();
        }

        private void TrayIcon_Clicked(object sender, EventArgs e)
        {
            _flyoutService.Toggle();
        }

        public void Dispose()
        {
            _trayIcon?.Dispose();
            _win32MessageGrabber?.Dispose();
        }

        #region Windows Taskbar Refresh
        private readonly Win32MessageGrabber _win32MessageGrabber = new Win32MessageGrabber();
        public class Win32MessageGrabber : NativeWindow, IDisposable
        {
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            static extern uint RegisterWindowMessage(string lpString);

            public event EventHandler TaskbarCreated;

            private static uint WM_TASKBARCREATED = RegisterWindowMessage("TaskbarCreated");
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

                // check if we got a hot key pressed.
                if (m.Msg == WM_TASKBARCREATED)
                {
                    TaskbarCreated?.Invoke(this, EventArgs.Empty);
                }
            }

            #region IDisposable Members

            public void Dispose()
            {
                DestroyHandle();
            }

            #endregion
        }
        #endregion
    }
}
