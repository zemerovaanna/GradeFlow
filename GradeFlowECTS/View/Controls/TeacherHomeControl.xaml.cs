using System.Windows.Controls;
using GradeFlowECTS.View.Windows;
using GradeFlowECTS.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.View.Controls
{
    public partial class TeacherHomeControl : UserControl
    {
        public TeacherHomeControl()
        {
            InitializeComponent();

            TeacherHomeViewModel teacherHomeViewModel = App.Current.ServiceProvider.GetRequiredService<TeacherHomeViewModel>();

            teacherHomeViewModel.EditExamAction = exam =>
            {
                EditExamWindow editWindow = new EditExamWindow(exam);
                if (editWindow.ShowDialog() == true)
                {
                    teacherHomeViewModel.RefreshExams();
                }
            };
            
            teacherHomeViewModel.GoExamAction = () =>
            {
                ExamWindow window = new ExamWindow();
                window.ShowDialog();
            };
            
            DataContext = teacherHomeViewModel;
        }

        private void CriteriaManagementButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            EditCriteriaWindow window = new EditCriteriaWindow();
            window.ShowDialog();
        }

        private void VariantManagementButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            VariantManagmentWindow window = new VariantManagmentWindow();
            window.ShowDialog();
        }
    }
}