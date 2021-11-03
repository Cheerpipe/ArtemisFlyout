using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ArtemisFlyout.Services.FlyoutServices;
using ArtemisFlyout.ViewModels;
using Avalonia;
using Avalonia.Markup.Xaml;
using Ninject;
using ReactiveUI;
// ReSharper disable UnusedParameter.Local

namespace ArtemisFlyout.Views
{
    public class FlyoutContainer : FlyoutWindow<FlyoutContainerViewModel>
    {

        public FlyoutContainer()
        {
            this.WhenActivated(disposables =>
            {
                /* Handle interactions etc. */
            });
            AvaloniaXamlLoader.Load(this);

#if DEBUG
            this.AttachDevTools();
#endif
            //_flyoutService = flyoutService;

            HorizontalSpacing = 12;
            AnimationDelay = 250;
            Width = 320;
            Height = 510;
            Deactivated += (_, _) =>
            {
                CloseAnimated();
            };
        }
    }
}