using System.Windows;

namespace GradeFlowECTS.View.Windows
{
    public partial class StudentPracticeResultWindow : Window
    {
        public StudentPracticeResultWindow(string result)
        {
            InitializeComponent();
            ResultText.Text = result;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}