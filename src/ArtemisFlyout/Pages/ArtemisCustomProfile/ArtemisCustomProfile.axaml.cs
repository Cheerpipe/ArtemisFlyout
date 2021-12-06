using System;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Transformation;
using Avalonia.ReactiveUI;

namespace ArtemisFlyout.Pages
{
    public  class ArtemisCustomProfile : ReactiveUserControl<ArtemisDeviceTogglesViewModel>
    {
        public ArtemisCustomProfile()
        {
            InitializeComponent();
            this.AttachedToVisualTree += ArtemisCustomProfile_AttachedToVisualTree;
        }

        private void ArtemisCustomProfile_AttachedToVisualTree(object sender, Avalonia.VisualTreeAttachmentEventArgs e)
        {
            Panel contentPanel = this.Find<Panel>("ContentPanel");
            TransformOperationsTransition contentTransition = new TransformOperationsTransition()
            {
                Property = Panel.RenderTransformProperty,
                Duration = TimeSpan.FromMilliseconds(1000),
                Easing = new ExponentialEaseOut()
            };
            contentTransition.Apply(contentPanel, Avalonia.Animation.Clock.GlobalClock, TransformOperations.Parse("translate(-20px, 0px)"), TransformOperations.Parse("translate(0px, 0px)"));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
