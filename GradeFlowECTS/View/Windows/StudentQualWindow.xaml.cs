using System.Windows;
using GradeFlowECTS.Analyzers;

namespace GradeFlowECTS.View.Windows
{
    public partial class StudentQualWindow : Window
    {
        public StudentQualWindow()
        {
            InitializeComponent();
            DataContext = new AnalyzerViewModel();
        }
    }
}