using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using Avalonia.Media;

namespace ArtemisFlyout.UserControls
{
    public class ArtemisCustomProfileViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        public ArtemisCustomProfileViewModel(IArtemisService artemisService)
        {
            _artemisService = artemisService;
        }

        public Color Foreground
        {
            get => _artemisService.GetColor("ForegroundColor", Color.Parse("#FF00FCCC"));
            set => _artemisService.SetColor("ForegroundColor",value);
        }

        public Color Background
        {
            get => _artemisService.GetColor("BackgroundColor", Color.Parse("#FFFF6D00"));
            set => _artemisService.SetColor("BackgroundColor", value);
        }

        public Color KeyboardAccentColor
        {
            get => _artemisService.GetColor("KeyboardAccentColor", Color.Parse("#FFEF1788"));
            set => _artemisService.SetColor("KeyboardAccentColor", value);
        }

        public Color EffectsAccentColor
        {
            get => _artemisService.GetColor("EffectsAccentColor", Color.Parse("#FFEF1788"));
            set => _artemisService.SetColor("EffectsAccentColor", value);
        }
    }
}
