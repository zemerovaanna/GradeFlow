using System.Windows;

namespace GradeFlowECTS.Interfaces
{
    public interface IWindowService
    {
        void CloseWindow(Window window);
        void CloseWindow<TWindow>() where TWindow : Window;
    }
}