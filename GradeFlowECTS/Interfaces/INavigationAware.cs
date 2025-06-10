using GradeFlowECTS.Core;

namespace GradeFlowECTS.Interfaces
{
    public interface INavigationAware
    {
        void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;
    }
}