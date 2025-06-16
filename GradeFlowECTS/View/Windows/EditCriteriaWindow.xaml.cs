using System.Windows;
using System.Windows.Controls;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Models;
using GradeFlowECTS.ViewModel;
using GradeFlowECTS.ViewModel.Items;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.View.Windows
{
    public partial class EditCriteriaWindow : Window
    {
        private EditCriteriaViewModel _viewModel;
        public List<Module> Modules { get; } = LoadModules();
        private static List<Module> LoadModules()
        {
            var context = App.Current.ServiceProvider.GetRequiredService<GradeFlowContext>();
            return context.Modules.ToList();
        }

        public EditCriteriaWindow()
        {
            InitializeComponent();
            _viewModel = new EditCriteriaViewModel();
            DataContext = _viewModel;
        }

        private void DeleteCriterion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is CriterionViewModel criterion)
            {
                _viewModel.DeleteCriterion(criterion);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveChanges();
            MessageBox.Show("Изменения сохранены", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddCriterionWindow window = new AddCriterionWindow(Modules);
            if (window.ShowDialog() == true)
            {
                _viewModel.LoadData();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}