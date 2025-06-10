using System.Windows.Input;
using GradeFlowECTS.Core;
using GradeFlowECTS.Interfaces;

namespace GradeFlowECTS.ViewModel
{
    public class InterludeViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationService _navigationService;

        public event EventHandler? CloseRequested;

        public BaseViewModel? CurrentViewModel => _navigationService.CurrentViewModel;

        public ICommand CloseWindowCommand { get; }

        public InterludeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;

            CloseWindowCommand = new RelayCommand(RequestClose);
        }

        public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel => _navigationService.NavigateTo<TViewModel>();

        private void OnCurrentViewModelChanged() => OnPropertyChanged(nameof(CurrentViewModel));

        private void RequestClose(object? parameter) => CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}