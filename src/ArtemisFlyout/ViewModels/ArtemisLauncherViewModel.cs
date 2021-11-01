using System.Diagnostics;
using System.Reactive.Disposables;
using ArtemisFlyout.Services;
using ReactiveUI;

namespace ArtemisFlyout.ViewModels
{
    public class ArtemisLauncherViewModel : ViewModelBase
    {
        public ArtemisLauncherViewModel()
        {
            this.WhenActivated(disposables =>
            {
                Disposable
                    .Create(() =>
                    {
                    })
                    .DisposeWith(disposables);
            });
        }
    }
}
