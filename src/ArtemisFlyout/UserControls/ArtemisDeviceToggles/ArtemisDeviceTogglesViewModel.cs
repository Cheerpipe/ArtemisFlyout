using System.Collections.Generic;
using System.Reactive.Disposables;
using ArtemisFlyout.Models.Configuration;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using ReactiveUI;

namespace ArtemisFlyout.UserControls
{

    public class ArtemisDeviceTogglesViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly IConfigurationService _configurationService;
        private List<Blackout> _blackoutSettings;
        private List<BlackoutViewModel> _blackouts;

        public ArtemisDeviceTogglesViewModel(IArtemisService artemisService, IConfigurationService configurationService)
        {
            _artemisService = artemisService;
            _configurationService = configurationService;
            _ceiling = _artemisService.GetJsonDataModelValue("Blackouts", "MoonBlackout", false);
            _blackoutSettings = _configurationService.Get().Blackouts;
            _blackouts = new();
            foreach (var blackout in _blackoutSettings)
            {
                _blackouts.Add(new BlackoutViewModel(blackout));
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

        private bool _ceiling;
        public bool Ceiling
        {
            get => _artemisService.GetJsonDataModelValue("Blackouts", "MoonBlackout", false);
            set
            {
                this.
                _artemisService.SetJsonDataModelValue("Blackouts", "MoonBlackout", value);
                this.RaiseAndSetIfChanged(ref _ceiling, value);
            }
        }

        public bool Television
        {
            get => _artemisService.GetJsonDataModelValue("Blackouts", "TvBlackout", false);
            set => this._artemisService.SetJsonDataModelValue("Blackouts", "TvBlackout", value);
        }

        public List<BlackoutViewModel> Blackouts => _blackouts;

        public void BlackoutStateChange()
        {

        }

        public bool Display
        {
            get => _artemisService.GetJsonDataModelValue("Blackouts", "DisplayBlackout", false);
            set => this._artemisService.SetJsonDataModelValue("Blackouts", "DisplayBlackout", value);
        }

        public bool Speakers
        {
            get => _artemisService.GetJsonDataModelValue("Blackouts", "SpeakerBlackout", false);
            set => this._artemisService.SetJsonDataModelValue("Blackouts", "SpeakerBlackout", value);
        }

        public bool DesktopBackside
        {
            get => _artemisService.GetJsonDataModelValue("Blackouts", "TableBackBlackout", false);
            set => this._artemisService.SetJsonDataModelValue("Blackouts", "TableBackBlackout", value);
        }

        public bool Peripherals
        {
            get => _artemisService.GetJsonDataModelValue("Blackouts", "PeripheralsBlackout", false);
            set => this._artemisService.SetJsonDataModelValue("Blackouts", "PeripheralsBlackout", value);
        }

        public bool DesktopSurface
        {
            get => _artemisService.GetJsonDataModelValue("Blackouts", "TableOverBlackout", false);
            set => this._artemisService.SetJsonDataModelValue("Blackouts", "TableOverBlackout", value);
        }

        public bool DesktopUnderside
        {
            get => _artemisService.GetJsonDataModelValue("Blackouts", "TableUnderBlackout", false);
            set => this._artemisService.SetJsonDataModelValue("Blackouts", "TableUnderBlackout", value);
        }

        public bool NightTable
        {
            get => _artemisService.GetJsonDataModelValue("Blackouts", "NightTableBlackout", false);
            set => this._artemisService.SetJsonDataModelValue("Blackouts", "NightTableBlackout", value);
        }

        public bool ComputerCase
        {
            get => _artemisService.GetJsonDataModelValue("Blackouts", "ComputerCaseBlackout", false);
            set => this._artemisService.SetJsonDataModelValue("Blackouts", "ComputerCaseBlackout", value);
        }
    }
}
