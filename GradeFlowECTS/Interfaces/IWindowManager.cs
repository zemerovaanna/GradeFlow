using GradeFlowECTS.Core;

namespace GradeFlowECTS.Interfaces
{
    public interface IWindowManager
    {
        void ShowWindow<TViewModel, TInitialViewModel>()
            where TViewModel : BaseViewModel
            where TInitialViewModel : BaseViewModel;
    }
}