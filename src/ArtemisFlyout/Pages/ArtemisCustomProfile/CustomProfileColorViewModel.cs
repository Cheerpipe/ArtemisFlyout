using ArtemisFlyout.IoC;
using ArtemisFlyout.Models.Configuration;
using ArtemisFlyout.Services;
using ArtemisFlyout.Utiles;
using ArtemisFlyout.ViewModels;
using Avalonia.Media;
using ReactiveUI;
using System;

namespace ArtemisFlyout.Pages
{
    public class CustomProfileColorViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private readonly string _customProfileColorsDatamodelName;
        private Color _color;

        public CustomProfileColorViewModel(CustomProfileColorSetting profileColor)
        {
            _artemisService = Kernel.Get<IArtemisService>();
            Name = profileColor.Name;
            Condition = profileColor.Condition;
            ShowInPreview = profileColor.ShowInPreview;
            _customProfileColorsDatamodelName = Kernel.Get<IConfigurationService>().Get().DatamodelSettings.CustomProfileColorsDatamodelName;

            var hexColor = _artemisService.GetJsonDataModelValue(_customProfileColorsDatamodelName, Condition, ColorUtiles.ToHexString(Colors.White));
            _color = Color.TryParse(hexColor, out Color parsedColor) ? parsedColor : Colors.White;

        }

        public Color Color
        {
            get => _color;
            set
            {
                _artemisService.SetJsonDataModelValue(_customProfileColorsDatamodelName, Condition, ColorUtiles.ToHexString(value));
                this.RaiseAndSetIfChanged(ref _color, value);
                ProfileColorChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public string Name { get; set; }
        public string Condition { get; set; }

        public bool ShowInPreview { get; set; }

        public event EventHandler ProfileColorChanged;

    }
}
