using System.Reactive.Disposables;
using ArtemisFlyout.Services.ArtemisServices;
using ArtemisFlyout.ViewModels;
using ReactiveUI;

namespace ArtemisFlyout.UserControls
{
    public class ArtemisDeviceTogglesViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;

        
        public ArtemisDeviceTogglesViewModel(IArtemisService artemisService)
        {
            _artemisService = artemisService;
            _ceiling= _artemisService.GetJsonDataModelValue("Blackouts", "MoonBlackout", false);

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
