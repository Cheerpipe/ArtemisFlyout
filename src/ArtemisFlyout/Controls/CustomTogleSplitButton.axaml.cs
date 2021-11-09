#nullable enable
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ArtemisFlyout.Controls
{
    public class CustomToggleSplitButton : UserControl
    {

        //TODO: Have to learn how to create a real custom control :D and don't mix code behind with xaml
        public CustomToggleSplitButton()
        {
            InitializeComponent();

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        private void ButtonMore_OnClick(object sender, RoutedEventArgs e)
        {
            ToggleButton buttonMore = (ToggleButton)e.Source!;
            buttonMore.IsChecked = true;
        }
    }
}
