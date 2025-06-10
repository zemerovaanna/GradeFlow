using System.Windows;
using GradeFlowECTS.Models;
using GradeFlowECTS.View.Pages;

namespace GradeFlowECTS.View.Windows
{
    public partial class ExamWindow : Window
    {
        public ExamWindow()
        {
            InitializeComponent();
            examFrame.Navigate(new ExamPage());
        }
    }
}