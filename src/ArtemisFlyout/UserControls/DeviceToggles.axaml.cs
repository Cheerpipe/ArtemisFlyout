        using ArtemisFlyout.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ArtemisFlyout.UserControls
{
    public partial class DeviceToggles : UserControl
    {
        public DeviceToggles()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.DataContext = new DeviceTogglesViewModel();

            AvaloniaXamlLoader.Load(this);
        }

        private void BtnBack_OnClick(object? sender, RoutedEventArgs e)
        {
            Program.MainWindowInstance.SetContentPageIndex(0);
        }
    }
}
