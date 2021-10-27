using System.Diagnostics;
using ArtemisFlyout.ViewModels;
using Avalonia;
using Avalonia.Controls;
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
            if (processName.Length != 0)
                this.DataContext = new ArtemisControlViewModel();
            AvaloniaXamlLoader.Load(this);
        }
    }
}
