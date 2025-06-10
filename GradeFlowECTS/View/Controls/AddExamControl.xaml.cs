using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace GradeFlowECTS.View.Controls
{
    public partial class AddExamControl : UserControl
    {
        public AddExamControl()
        {
            InitializeComponent();
        }

        private static readonly Regex NumberOnly = new Regex("^[0-9]+$");

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberOnly.IsMatch(e.Text);
        }
    }
}