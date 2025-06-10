using System.Windows;
using GradeFlowECTS.Models;
using GradeFlowECTS.ViewModel.Items;

namespace GradeFlowECTS.View.Windows
{
    public partial class GroupSelectionWindow : Window
    {
        public GroupItemViewModel? SelectedGroupVM { get; private set; }

        public GroupSelectionWindow(IEnumerable<GroupItemViewModel> groupVMs)
        {
            InitializeComponent();
            GroupComboBox.ItemsSource = groupVMs;
            GroupComboBox.SelectedIndex = 0;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedGroupVM = GroupComboBox.SelectedItem as GroupItemViewModel;
            if (SelectedGroupVM == null)
            {
                MessageBox.Show("Пожалуйста, выберите группу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
            Close();
        }
    }
}