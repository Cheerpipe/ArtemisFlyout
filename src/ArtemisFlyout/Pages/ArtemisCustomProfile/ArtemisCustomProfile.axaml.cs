using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace ArtemisFlyout.Pages
{
    public  class ArtemisCustomProfile : ReactiveUserControl<ArtemisDeviceTogglesViewModel>
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
