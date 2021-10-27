using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ArtemisFlyout.UserControls
{
    public partial class ArtemisLauncher : UserControl
    {
        private const string ArtemisPath = @"D:\Repos\Artemis\src\Artemis.UI\bin\net5.0-windows\Artemis.UI.exe";
        private const string ArtemisParams = "--minimized --pcmr --ignore-plugin-lock";

        public ArtemisLauncher()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void BtnLaunch_OnClick(object? sender, RoutedEventArgs e)
        {
            Process.Start(ArtemisPath, ArtemisParams);
            Program.MainWindowInstance.Close();
        }
    }
}
