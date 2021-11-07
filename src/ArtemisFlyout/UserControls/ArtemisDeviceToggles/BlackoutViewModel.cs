using ArtemisFlyout.IoC;
using ArtemisFlyout.Models.Configuration;
using ArtemisFlyout.Services;
using ArtemisFlyout.ViewModels;
using ReactiveUI;

namespace ArtemisFlyout.UserControls
{
    public class BlackoutViewModel : ViewModelBase
    {
        private readonly IArtemisService _artemisService;
        private bool _activated;

        public BlackoutViewModel(Blackout blackout)
        {
            Name = blackout.Name;
            Condition = blackout.Condition;
            _artemisService = Kernel.Get<IArtemisService>();
        }

        public bool Activated
        {
            get => _artemisService.GetJsonDataModelValue("Blackouts", Condition, false);
            set
            {
                _artemisService.SetJsonDataModelValue("Blackouts", Condition, value);
                this.RaiseAndSetIfChanged(ref _activated, value);
            }
        }
        public string Name { get; set; }
        public string Condition { get; set; }
    }
}
