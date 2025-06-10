using GradeFlowECTS.Core;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.View.Windows;
using GradeFlowECTS.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Threading;

namespace GradeFlowECTS.Infrastructure
{
    public class WindowManager : IWindowManager
    {
        private readonly IServiceProvider _serviceProvider;

        public WindowManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void ShowWindow<TViewModel, TInitialViewModel>()
            where TViewModel : BaseViewModel
            where TInitialViewModel : BaseViewModel
        {
            var scope = _serviceProvider.CreateScope();
            var viewModel = scope.ServiceProvider.GetRequiredService<TViewModel>();

            if (viewModel is INavigationAware navigationAware)
            {
                navigationAware.NavigateTo<TInitialViewModel>();
            }

            Window? window = null;

            if (typeof(TViewModel) == typeof(InterludeViewModel))
            {
                window = new InterludeWindow(viewModel as InterludeViewModel);
            }

            if (window != null)
            {
                window.Closed += (s, e) => scope.Dispose();
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    window.Show();
                }, DispatcherPriority.ApplicationIdle);
            }
        }
    }
}