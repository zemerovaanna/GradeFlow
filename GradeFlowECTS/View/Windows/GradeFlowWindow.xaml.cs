using System.Windows;
using GradeFlowECTS.ViewModel;

namespace GradeFlowECTS.View.Windows
{
    public partial class GradeFlowWindow : Window
    {
        public GradeFlowWindow(GradeFlowViewModel gradeFlowViewModel)
        {
            InitializeComponent();
            DataContext = gradeFlowViewModel;
        }
    }
}