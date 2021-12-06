using System;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Transformation;
using Avalonia.ReactiveUI;
using ReactiveUI;

// ReSharper disable UnusedParameter.Local

namespace ArtemisFlyout.Pages
{
    public class ArtemisDeviceToggles : ReactiveUserControl<ArtemisDeviceTogglesViewModel>
    {
        public ArtemisDeviceToggles()
        {
            // If you put a WhenActivated block here, your activatable view model 
            // will also support activation, otherwise it won't.
            this.WhenActivated(disposables =>
            {
            });
            AvaloniaXamlLoader.Load(this);
            AttachedToVisualTree += ArtemisDeviceToggles_AttachedToVisualTree;
        }


        private void ArtemisDeviceToggles_AttachedToVisualTree(object sender, Avalonia.VisualTreeAttachmentEventArgs e)
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
    }
}
