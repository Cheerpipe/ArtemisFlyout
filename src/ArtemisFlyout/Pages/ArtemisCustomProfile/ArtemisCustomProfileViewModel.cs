using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using Avalonia.Media;
using ReactiveUI;

namespace ArtemisFlyout.Pages
{
    public class ArtemisCustomProfileViewModel : ViewModelBase
    {
        private readonly List<CustomProfileColorViewModel> _customProfileColors;
        public ArtemisCustomProfileViewModel(IConfigurationService configurationService)
        {
            var customProfileColorsSetting = configurationService.Get().CustomProfilesColorSettings;

            _customProfileColors = new();
            foreach (var customProfileColor in customProfileColorsSetting)
            {
                CustomProfileColorViewModel customProfileColorViewModel = new CustomProfileColorViewModel(customProfileColor);
                _customProfileColors.Add(customProfileColorViewModel);
                customProfileColorViewModel.ProfileColorChanged += CustomProfileColorViewModel_ProfileColorChanged;
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

        private void CustomProfileColorViewModel_ProfileColorChanged(object sender, System.EventArgs e)
        {
            this.RaisePropertyChanged(nameof(PreviewColor));
        }

        public List<CustomProfileColorViewModel> ProfileColors => _customProfileColors;

        public double CalculatedHeight => (65 * ProfileColors.Count) + 130;


        public IBrush PreviewColor
        {
            get
            {
                List<CustomProfileColorViewModel> previewColors = ProfileColors.Where(c => c.ShowInPreview).ToList();
                if (previewColors.Count == 0)
                    return new SolidColorBrush(Colors.Transparent);
                else
                {
                    LinearGradientBrush brush = new LinearGradientBrush();
                    int stopCount = 0;
                    foreach (var colorVm in previewColors)
                    {
                        brush.GradientStops.Add(new GradientStop(colorVm.Color, stopCount * (1d / (previewColors.Count - 1))));
                        stopCount++;
                    }
                    return brush;
                }
            }
        }
    }
}
