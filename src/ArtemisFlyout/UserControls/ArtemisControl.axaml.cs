using System.Diagnostics;
using ArtemisFlyout.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ArtemisFlyout.UserControls
{
    public partial class ArtemisControl : UserControl
    {
        public ArtemisControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var processName = Process.GetProcessesByName("Artemis.UI");

            // Don't load VM if artemis is not running
            if (processName.Length != 0)
                this.DataContext = new ArtemisControlViewModel();
            AvaloniaXamlLoader.Load(this);
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            Program.MainWindowInstance.SetContentPageIndex(2);
        }
    }
}
