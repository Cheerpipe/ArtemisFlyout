using System.Reactive.Disposables;
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
