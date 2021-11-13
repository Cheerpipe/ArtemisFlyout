using ArtemisFlyout.IoC;
using ArtemisFlyout.Models.Configuration;
using ArtemisFlyout.Services;
using ArtemisFlyout.Utiles;
using ArtemisFlyout.ViewModels;
using Avalonia.Media;
using ReactiveUI;

namespace ArtemisFlyout.Pages
{
    public class CustomProfileColorViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly string _customProfileColorsDatamodelName;
        private Color _color;

        public CustomProfileColorViewModel(CustomProfileColorSetting device)
        {
            _artemisService = Kernel.Get<IArtemisService>();
            Name = device.Name;
            Condition = device.Condition;
            _customProfileColorsDatamodelName = Kernel.Get<IConfigurationService>().Get().DatamodelSettings.CustomProfileColorsDatamodelName;

            var hexColor = _artemisService.GetJsonDataModelValue(_customProfileColorsDatamodelName, Condition, ColorUtiles.ToHexString(Colors.White));
            if (Color.TryParse(hexColor, out Color parsedColor))
                _color = parsedColor;
            else
                _color = Colors.White;

        }

        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _artemisService.SetJsonDataModelValue(_customProfileColorsDatamodelName, Condition,
                    ColorUtiles.ToHexString(value));
                this.RaiseAndSetIfChanged(ref _color, value);
            }
        }

        public string Name { get; set; }
        public string Condition { get; set; }

    }
}
