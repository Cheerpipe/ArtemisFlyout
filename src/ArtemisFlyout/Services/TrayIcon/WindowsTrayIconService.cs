using System;
using System.Windows.Forms;
using ArtemisFlyout.Platform.Windows;

namespace ArtemisFlyout.Services
{
    internal class WindowsTrayIconService : TrayIconService, IDisposable
    {
        private static uint WM_TASKBARCREATED = Win32MessageGrabber.RegisterWindowMessage("TaskbarCreated");
        private readonly Win32MessageGrabber _win32MessageGrabber = new Win32MessageGrabber();

        public WindowsTrayIconService(IFlyoutService flyoutService) : base(flyoutService)
        {
            _win32MessageGrabber.MessageReceived += _win32MessageGrabber_MessageReceived;
        }


        private void _win32MessageGrabber_MessageReceived(object sender, Message e)
        {
            if (e.Msg == WM_TASKBARCREATED)
                Refresh();
        }

        public new void Dispose()
        {
            base.Dispose();
            _win32MessageGrabber?.Dispose();
        }
    }
}
