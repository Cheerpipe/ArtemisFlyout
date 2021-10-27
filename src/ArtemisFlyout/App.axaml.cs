using System;
using System.Threading;
using ArtemisFlyout.ViewModels;
using ArtemisFlyout.Views;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Application = Avalonia.Application;

namespace ArtemisFlyout
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();
          //  CancellationToken t = new CancellationToken();
          //  Avalonia.Application.Current.Run(t);
        }
    }
}
