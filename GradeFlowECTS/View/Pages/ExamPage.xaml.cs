using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.Services;
using GradeFlowECTS.View.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace GradeFlowECTS.View.Pages
{
    public partial class ExamPage : Page
    {
        private readonly ExamRepository _examRepository;
        private readonly Exam _exam;
        private readonly IUserContext _userContext;
        GradeFlowContext context;

        public ExamPage()
        {
            InitializeComponent();

            TeacherMDK.Visibility = Visibility.Collapsed;
            TeacherQualificationExam.Visibility = Visibility.Collapsed;
            StudentMDK.Visibility = Visibility.Collapsed;
            StudentQualificationExam.Visibility = Visibility.Collapsed;

            _userContext = App.Current.ServiceProvider.GetRequiredService<IUserContext>();
            int roleId = _userContext.CurrentUser.RoleId;
            context = App.Current.ServiceProvider.GetRequiredService<GradeFlowContext>();
            AesGcmCryptographyService cryptographyService = new AesGcmCryptographyService();
            FileService fileService = new FileService("GradeFlow");
            UserSettingsService userSettingsService = new UserSettingsService(fileService);
            ConfigurationService configurationService = new ConfigurationService(cryptographyService, fileService, userSettingsService);
            UserRoleRepository userRoleRepository = new UserRoleRepository(context, configurationService, cryptographyService);
            _examRepository = new ExamRepository();
            IExamContext examContext = App.Current.ServiceProvider.GetRequiredService<IExamContext>();
            _exam = _examRepository.GetExamById(examContext.CurrentExamId);
            DataContext = _exam;

            TimeToCompleteOkDa.Text = Convert.ToString(context.ExamPractices.Where(e => e.ExamId == _exam.ExamId).Select(e => e.PracticeTimeToComplete).FirstOrDefault());

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
                        ObosraniContainer.Visibility = Visibility.Collapsed;
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

        public void UpdateExam()
        {
            try
            {
                //updatedExam.PracticeTimeToComplete = Convert.ToInt32(TimeToCompleteOkDa.Text);
                var existingExam = context.ExamPractices
                    .FirstOrDefault(et => et.ExamId == _exam.ExamId);

                if (existingExam != null && !TimeToCompleteOkDa.Text.IsNullOrEmpty())
                {
                    existingExam.PracticeTimeToComplete = Convert.ToInt32(TimeToCompleteOkDa.Text);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UpdateExam] Ошибка: {ex.Message}");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateExam();
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
            TestResultWindow window = new TestResultWindow(_exam, _examRepository);
            window.ShowDialog();
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
            QualResultsWindow window = new QualResultsWindow(_exam, _examRepository);
            window.ShowDialog();
        }

        private void EditQualificationExamButton_Click(object sender, RoutedEventArgs e)
        {
            VariantWindow window = new VariantWindow(_exam);
            window.ShowDialog();
        }

        private void TakeTestButton_Click(object sender, RoutedEventArgs e)
        {
            var context = new GradeFlowContext();
            var mhm = context.StudentExamResults.Where(s => s.ExamId == _exam.ExamId).FirstOrDefault();
            if (mhm == null || mhm.TestStart == false)
            {
                var user = App.Current.ServiceProvider.GetRequiredService<IUserContext>();
                var student = context.Students.FirstOrDefault(s => s.StudentId == user.CurrentUser.StudentId);

                if (student != null)
                {
                    DateTime now = DateTime.Now;
                    TimeOnly currentTime = TimeOnly.FromDateTime(now);
                    DateOnly currentDate = DateOnly.FromDateTime(now);

                    // Поиск существующего результата
                    Guid examId = _exam.ExamId;

                    var existingResult = context.StudentExamResults
                        .FirstOrDefault(r => r.StudentId == student.StudentId && r.ExamId == examId);

                    if (existingResult != null)
                    {
                        // Обновление существующего результата
                        existingResult.TimeEnded = currentTime;
                        existingResult.DateEnded = currentDate;
                        existingResult.TestStart = true;

                        context.StudentExamResults.Update(existingResult);
                    }
                    else
                    {
                        // Создание нового результата
                        StudentExamResult newResult = new StudentExamResult
                        {
                            StudentId = student.StudentId,
                            ExamId = _exam.ExamId,
                            TimeEnded = currentTime,
                            DateEnded = currentDate,
                            TestStart = true
                        };

                        context.StudentExamResults.Add(newResult);
                    }

                    context.SaveChanges();

                    StudentTestWindow window = new StudentTestWindow(_examRepository, _exam);
                    window.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Тест уже пройден.");
            }
        }

        private void CompletePracticalTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var context = new GradeFlowContext();
            var mhm = context.StudentExamResults.Where(s => s.ExamId == _exam.ExamId).FirstOrDefault();
            if (mhm == null || mhm.PracticeTimeSpent.IsNullOrEmpty())
            {
                if (_exam.Discipline.DisciplineName == "МДК 01.01")
                {
                    StudentMDK01Window window = new StudentMDK01Window(_userContext.CurrentUser.StudentId ?? 0, _exam.ExamId);
                    window.ShowDialog();
                }
                else if (_exam.Discipline.DisciplineName == "МДК 01.02")
                {
                    StudentMDK02Window window = new StudentMDK02Window(_userContext.CurrentUser.StudentId ?? 0, _exam.ExamId);
                    window.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Практическое задание уже пройдено.");
            }
        }

        private void TakeQualificationExamButton_Click(object sender, RoutedEventArgs e)
        {
            var context = new GradeFlowContext();
            var mhm = context.StudentExamResults.Where(s => s.ExamId == _exam.ExamId).FirstOrDefault();
            if (mhm == null || mhm.PracticeTimeSpent.IsNullOrEmpty())
            {
                StudentQualWindow window = new StudentQualWindow(_userContext.CurrentUser.StudentId ?? 0, _exam.ExamId);
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Уже пройдено.");
            }
        }

        private static readonly Regex NumberOnly = new Regex("^[0-9]+$");
        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberOnly.IsMatch(e.Text);
        }
    }
}