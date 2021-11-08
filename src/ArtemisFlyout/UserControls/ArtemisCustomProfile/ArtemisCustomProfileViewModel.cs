using System.Collections.Generic;
using System.Reactive.Disposables;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;

using ReactiveUI;

namespace ArtemisFlyout.UserControls
{
    public class ArtemisCustomProfileViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly List<CustomProfileColorViewModel> _customProfileColors;
        public ArtemisCustomProfileViewModel(IConfigurationService configurationService)
        {
            var customProfileColorsSetting = configurationService.Get().CustomProfilesColorSettings;

            _customProfileColors = new();
            foreach (var customProfileColor in customProfileColorsSetting)
            {
                _customProfileColors.Add(new CustomProfileColorViewModel(customProfileColor));
            }

            this.WhenActivated(disposables =>
            {
                Disposable
                    .Create(() =>
                    {
                    })
                    .DisposeWith(disposables);
            });
        }
        public List<CustomProfileColorViewModel> Colors => _customProfileColors;
    }
}
