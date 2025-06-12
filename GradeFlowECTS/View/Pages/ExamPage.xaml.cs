using System.Windows;
using System.Windows.Controls;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.Services;
using GradeFlowECTS.View.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.View.Pages
{
    public partial class ExamPage : Page
    {
        private readonly ExamRepository _examRepository;
        private readonly Exam _exam;
        private readonly IUserContext _userContext;


        public ExamPage()
        {
            InitializeComponent();

            TeacherMDK.Visibility = Visibility.Collapsed;
            TeacherQualificationExam.Visibility = Visibility.Collapsed;
            StudentMDK.Visibility = Visibility.Collapsed;
            StudentQualificationExam.Visibility = Visibility.Collapsed;

            _userContext = App.Current.ServiceProvider.GetRequiredService<IUserContext>();
            int roleId = _userContext.CurrentUser.RoleId;
            GradeFlowContext context = App.Current.ServiceProvider.GetRequiredService<GradeFlowContext>();
            AesGcmCryptographyService cryptographyService = new AesGcmCryptographyService();
            FileService fileService = new FileService("GradeFlow");
            UserSettingsService userSettingsService = new UserSettingsService(fileService);
            ConfigurationService configurationService = new ConfigurationService(cryptographyService, fileService, userSettingsService);
            UserRoleRepository userRoleRepository = new UserRoleRepository(context, configurationService, cryptographyService);
            _examRepository = new ExamRepository();
            IExamContext examContext = App.Current.ServiceProvider.GetRequiredService<IExamContext>();
            _exam = _examRepository.GetExamById(examContext.CurrentExamId);
            DataContext = _exam;

            string roleName = userRoleRepository.GetRoleNameById(roleId);
            switch (roleName)
            {
                case "Преподаватель":
                    {
                        if (_exam.Discipline.DisciplineName != "Квалификационный экзамен")
                        {
                            TeacherMDK.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            TeacherQualificationExam.Visibility = Visibility.Visible;
                        }
                        break;
                    }
                case "Студент":
                    {
                        if (_exam.Discipline.DisciplineName != "Квалификационный экзамен")
                        {
                            StudentMDK.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            StudentQualificationExam.Visibility = Visibility.Visible;
                        }
                        break;
                    }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window? window = Window.GetWindow(this);
            window?.Close();
        }

        private void PassTestButton_Click(object sender, RoutedEventArgs e)
        {
            if(_examRepository.HasAnyQuestion(_exam) == false)
            {
                MessageBox.Show("Внимание, для тестовой части отсуствуют вопросы. Создать вопросы можно по кнопке \"Редактировать тест\"", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                NavigationService?.Navigate(new TeacherTestPage(_examRepository));
            }
        }

        private void ViewTestResultsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditTestButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new TestManagementPage(_examRepository));
        }

        private void ViewPracticalResultsButton_Click(object sender, RoutedEventArgs e)
        {
            MDK01ResultsWindow window = new MDK01ResultsWindow(_exam, _examRepository);
            window.ShowDialog();
        }

        private void ViewAttemptsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditQualificationExamButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TakeTestButton_Click(object sender, RoutedEventArgs e)
        {
            StudentTestWindow window = new StudentTestWindow(_examRepository);
            window.ShowDialog();
        }

        private void CompletePracticalTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if(_exam.Discipline.DisciplineName == "МДК 01.01")
            {
                StudentMDK01Window window = new StudentMDK01Window(_userContext.CurrentUser.StudentId ?? 0, _exam.ExamId);
                window.ShowDialog();
            }
            else if(_exam.Discipline.DisciplineName == "МДК 01.02")
            {
                StudentMDK02Window window = new StudentMDK02Window(_userContext.CurrentUser.StudentId ?? 0, _exam.ExamId);
                window.ShowDialog();
            }
        }

        private void TakeQualificationExamButton_Click(object sender, RoutedEventArgs e)
        {
            StudentQualWindow window = new StudentQualWindow(_userContext.CurrentUser.StudentId ?? 0, _exam.ExamId);
            window.ShowDialog();
        }
    }
}