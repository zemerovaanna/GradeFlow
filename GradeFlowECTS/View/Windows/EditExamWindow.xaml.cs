using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.Services;
using GradeFlowECTS.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.View.Windows
{
    public partial class EditExamWindow : Window
    {
        public EditExamWindow(Exam exam)
        {
            InitializeComponent();

            GradeFlowContext context = App.Current.ServiceProvider.GetRequiredService<GradeFlowContext>();
            AesGcmCryptographyService cryptographyService = new AesGcmCryptographyService();
            FileService fileService = new FileService("GradeFlow");
            UserSettingsService userSettingsService = new UserSettingsService(fileService);
            ConfigurationService configurationService = new ConfigurationService(cryptographyService, fileService, userSettingsService);
            DisciplineRepository disciplineRepository = new DisciplineRepository(context, cryptographyService, configurationService);
            ExamRepository examRepository = new ExamRepository();

            EditExamViewModel editExamViewModel = new EditExamViewModel(disciplineRepository, examRepository, new GroupRepository(context), new GroupsExamRepository(context), exam);

            editExamViewModel.SetRefreshAction = () => this.DialogResult = true;
            editExamViewModel.CancelAction = () => this.Close();

            DataContext = editExamViewModel;
        }

        private static readonly Regex NumberOnly = new Regex("^[0-9]+$");
        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberOnly.IsMatch(e.Text);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}