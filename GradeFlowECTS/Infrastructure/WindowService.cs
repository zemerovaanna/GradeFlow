using System.Windows;
using GradeFlowECTS.Interfaces;

namespace GradeFlowECTS.Infrastructure
{
    public class WindowService : IWindowService
    {
        public void CloseWindow(Window window)
        {
            window?.Close();
        }

        public void CloseWindow<TWindow>() where TWindow : Window
        {
            var window = Application.Current.Windows
                .OfType<TWindow>()
                .FirstOrDefault();

            CloseWindow(window);
        }
    }
}