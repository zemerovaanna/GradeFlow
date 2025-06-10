using GradeFlowECTS.Core;
using GradeFlowECTS.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.Infrastructure
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private BaseViewModel? _currentViewModel;

        public BaseViewModel? CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                _currentViewModel = value;
                CurrentViewModelChanged?.Invoke();
            }
        }

        public event Action? CurrentViewModelChanged;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel
        {
            CurrentViewModel =  _serviceProvider.GetRequiredService<TViewModel>();
        }
    }
}