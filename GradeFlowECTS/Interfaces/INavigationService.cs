using GradeFlowECTS.Core;

namespace GradeFlowECTS.Interfaces
{
    public interface INavigationService
    {
        BaseViewModel? CurrentViewModel { get; }
        event Action? CurrentViewModelChanged;
        void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;
    }
}