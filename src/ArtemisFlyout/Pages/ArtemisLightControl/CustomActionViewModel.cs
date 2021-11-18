using ArtemisFlyout.IoC;
using ArtemisFlyout.Services;

namespace ArtemisFlyout.Pages
{
    public class QuickActionViewModel
    {
        private readonly IArtemisService _artemisService;
        private readonly string _globalVariablesDatamodelName;

        public QuickActionViewModel(IArtemisService artemisService, IConfigurationService configurationService)
        {
            _artemisService = artemisService;
            _globalVariablesDatamodelName = configurationService.Get().DatamodelSettings.GlobalVariablesDatamodelName;
        }

        public string Name { get; set; }
        public string Condition { get; set; }
        public string Icon { get; set; }

        public bool State
        {
            get => _artemisService.GetJsonDataModelValue(_globalVariablesDatamodelName, Condition, false);
            set => _artemisService.SetJsonDataModelValue(_globalVariablesDatamodelName, Condition, value);
        }


    }
    public class QuickActionViewModelBuilder
    {
        private QuickActionViewModel _customActionViewModel = Kernel.Get<QuickActionViewModel>();

        public QuickActionViewModelBuilder WithName(string name)
        {
            _customActionViewModel.Name = name;
            return this;
        }

        public QuickActionViewModelBuilder WithCondition(string condition)
        {
            _customActionViewModel.Condition = condition;
            return this;
        }

        public QuickActionViewModelBuilder WithIcon(string icon)
        {
            _customActionViewModel.Icon = icon;
            return this;
        }

        public QuickActionViewModel Build()
        {
            return _customActionViewModel;
        }

    }
}
