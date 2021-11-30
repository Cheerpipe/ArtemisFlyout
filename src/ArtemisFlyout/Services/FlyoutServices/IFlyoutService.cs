using System;
using System.Threading.Tasks;
using ArtemisFlyout.ViewModels;

namespace ArtemisFlyout.Services
{
    public interface IFlyoutService
    {
        void Show(bool animate = true);
        Task CloseAndRelease(bool animate = true);
        void SetHeight(double newHeight);
        void SetWidth(double newWidth);
        Task PreLoad();
        void Toggle();

        void SetPopulateViewModelFunc(Func<ViewModelBase> populateViewModelFunc);
    }
}
