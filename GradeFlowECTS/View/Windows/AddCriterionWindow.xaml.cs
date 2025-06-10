using System.Windows;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.ViewModel;

namespace GradeFlowECTS.View.Windows
{
    public partial class AddCriterionWindow : Window
    {
        public AddCriterionViewModel ViewModel { get; }

        public Criterion? CreatedCriterion { get; private set; }

        public AddCriterionWindow(IEnumerable<Module> modules)
        {
            InitializeComponent();
            ViewModel = new AddCriterionViewModel(modules);
            DataContext = ViewModel;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ViewModel.CriterionTitle) || ViewModel.SelectedModule == null)
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            CreatedCriterion = ViewModel.ToCriterion();

            try
            {
                var repository = new CriterionRepository();
                repository.AddNewCriterion(CreatedCriterion);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении критерия в базу: " + ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}