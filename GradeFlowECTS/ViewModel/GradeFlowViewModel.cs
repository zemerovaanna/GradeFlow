using GradeFlowECTS.Core;
using GradeFlowECTS.Interfaces;

namespace GradeFlowECTS.ViewModel
{
    public class GradeFlowViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly IWindowManager _windowManager;

        public BaseViewModel? CurrentViewModel => _navigationService.CurrentViewModel;

        public GradeFlowViewModel(INavigationService navigationService, IUserSettingsService userSettingsService, IWindowManager windowManager)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _userSettingsService = userSettingsService ?? throw new ArgumentNullException(nameof(userSettingsService));
            _windowManager = windowManager ?? throw new ArgumentNullException(nameof(windowManager));

            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;

            HandleInitialNavigation();
        }

        private void HandleInitialNavigation()
        {
            if (_userSettingsService.IsFirstLaunch == true)
            {
                _windowManager.ShowWindow<InterludeViewModel, DbConnectionViewModel>();
            }
            _navigationService.NavigateTo<TeacherLoginViewModel>();
        }

        private void OnCurrentViewModelChanged() => OnPropertyChanged(nameof(CurrentViewModel));
    }
}