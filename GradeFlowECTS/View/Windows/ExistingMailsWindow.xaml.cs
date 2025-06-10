using System.Windows;

namespace GradeFlowECTS.View.Windows
{
    public partial class ExistingMailsWindow : Window
    {
        public ExistingMailsWindow(List<string> mails)
        {
            InitializeComponent();
            DataContext = mails;
        }
    }
}