using System.Reactive.Disposables;
using ArtemisFlyout.Services.ArtemisServices;
using ReactiveUI;

namespace ArtemisFlyout.ViewModels
{
    public class ArtemisDeviceTogglesViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;

        
        public ArtemisDeviceTogglesViewModel(IArtemisService artemisService)
        {
            _artemisService = artemisService;
            _ceiling= _artemisService.GetJsonDataModelValue<bool>("Blackouts", "MoonBlackout", false);

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
            get => _artemisService.GetJsonDataModelValue<bool>("Blackouts", "MoonBlackout", false);
            set
            {
                this.
                _artemisService.SetJsonDataModelValue<bool>("Blackouts", "MoonBlackout", value);
                this.RaiseAndSetIfChanged(ref _ceiling, value);
            }
        }

        public bool Television
        {
            get => _artemisService.GetJsonDataModelValue<bool>("Blackouts", "TvBlackout", false);
            set => this._artemisService.SetJsonDataModelValue<bool>("Blackouts", "TvBlackout", value);
        }

        public bool Display
        {
            get => _artemisService.GetJsonDataModelValue<bool>("Blackouts", "DisplayBlackout", false);
            set => this._artemisService.SetJsonDataModelValue<bool>("Blackouts", "DisplayBlackout", value);
        }

        public bool Speakers
        {
            get => _artemisService.GetJsonDataModelValue<bool>("Blackouts", "SpeakerBlackout", false);
            set => this._artemisService.SetJsonDataModelValue<bool>("Blackouts", "SpeakerBlackout", value);
        }

        public bool DesktopBackside
        {
            get => _artemisService.GetJsonDataModelValue<bool>("Blackouts", "TableBackBlackout", false);
            set => this._artemisService.SetJsonDataModelValue<bool>("Blackouts", "TableBackBlackout", value);
        }

        public bool Peripherals
        {
            get => _artemisService.GetJsonDataModelValue<bool>("Blackouts", "PeripheralsBlackout", false);
            set => this._artemisService.SetJsonDataModelValue<bool>("Blackouts", "PeripheralsBlackout", value);
        }

        public bool DesktopSurface
        {
            get => _artemisService.GetJsonDataModelValue<bool>("Blackouts", "TableOverBlackout", false);
            set => this._artemisService.SetJsonDataModelValue<bool>("Blackouts", "TableOverBlackout", value);
        }

        public bool DesktopUnderside
        {
            get => _artemisService.GetJsonDataModelValue<bool>("Blackouts", "TableUnderBlackout", false);
            set => this._artemisService.SetJsonDataModelValue<bool>("Blackouts", "TableUnderBlackout", value);
        }

        public bool NightTable
        {
            get => _artemisService.GetJsonDataModelValue<bool>("Blackouts", "NightTableBlackout", false);
            set => this._artemisService.SetJsonDataModelValue<bool>("Blackouts", "NightTableBlackout", value);
        }

        public bool ComputerCase
        {
            get => _artemisService.GetJsonDataModelValue<bool>("Blackouts", "ComputerCaseBlackout", false);
            set => this._artemisService.SetJsonDataModelValue<bool>("Blackouts", "ComputerCaseBlackout", value);
        }
    }
}
