using ArtemisFlyout.IoC;
using ArtemisFlyout.Models.Configuration;
using ArtemisFlyout.Services;
using ArtemisFlyout.Utiles;
using ArtemisFlyout.ViewModels;
using Avalonia.Media;
using ReactiveUI;

namespace ArtemisFlyout.UserControls
{
    public class CustomProfileColorViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly string _customProfileColorsDatamodelName;

        public CustomProfileColorViewModel(CustomProfileColorSetting device)
        {
            _artemisService = Kernel.Get<IArtemisService>();
            Name = device.Name;
            Condition = device.Condition;
            _customProfileColorsDatamodelName = Kernel.Get<IConfigurationService>().Get().DatamodelSettings.CustomProfileColorsDatamodelName;
        }

        private Color _color;

        public Color Color
        {
            get
            {
                var hexColor = _artemisService.GetJsonDataModelValue(_customProfileColorsDatamodelName, Condition, "");
                if (Color.TryParse(hexColor, out Color parsedColor))
                    return parsedColor;
                return Colors.White;
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
