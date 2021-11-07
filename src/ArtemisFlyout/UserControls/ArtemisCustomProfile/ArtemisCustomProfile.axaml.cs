using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace ArtemisFlyout.UserControls
{
    public partial class ArtemisCustomProfile : ReactiveUserControl<ArtemisDeviceTogglesViewModel>
    {
        public ArtemisCustomProfile()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
