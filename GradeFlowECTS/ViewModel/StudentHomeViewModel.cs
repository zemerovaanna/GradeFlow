using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GradeFlowECTS.Core;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.View.Windows;
using GradeFlowECTS.ViewModel.Items;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.ViewModel
{
    public class StudentHomeViewModel : BaseViewModel
    {
        private readonly IExamContext _examContext;
        private readonly IExamRepository _examRepository;
        private readonly INavigationService _navigationService;
        private ObservableCollection<ExamItemViewModel> _exams;

        public ObservableCollection<ExamItemViewModel> Exams
        {
            get => _exams;
            set => SetProperty(ref _exams, value);
        }

        public Visibility IsExamsEmpty => Exams.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

        public ICommand ExitAccountCommand { get; }
        public ICommand GoExamCommand { get; }

        public StudentHomeViewModel(IExamContext examContext, IExamRepository examRepository, INavigationService navigationService)
        {
            _examContext = examContext ?? throw new ArgumentNullException(nameof(examContext));
            _examRepository = examRepository ?? throw new ArgumentNullException(nameof(examRepository));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            ExitAccountCommand = new RelayCommand(ExitAccount);
            GoExamCommand = new RelayCommand(GoExam);

            RefreshExams();
        }

        public override void Dispose()
        {
            _navigationService.CurrentViewModelChanged -= OnCurrentViewModelChanged;
            base.Dispose();
        }

        private void ExitAccount(object? parameter) => _navigationService.NavigateTo<StudentLoginViewModel>();

        private void GoExam(object? parameter)
        {
            if (parameter is ExamItemViewModel examViewModel)
            {
                Exam? exam = _examRepository.GetExamById(examViewModel.ExamId);
                if (exam != null)
                {
                    _examContext.SetExamId(exam.ExamId);
                    ExamWindow window = new ExamWindow();
                    window.ShowDialog();
                }
            }
        }

        private void OnCurrentViewModelChanged()
        {
            if (_navigationService.CurrentViewModel is AddExamViewModel)
            {
                RefreshExams();
            }
        }

        public void RefreshExams()
        {
            IUserContext userContext = App.Current.ServiceProvider.GetRequiredService<IUserContext>();
            if (userContext.CurrentUser.StudentId != null)
            {
                Exams = new ObservableCollection<ExamItemViewModel>(_examRepository.GetUpcomingExamsForStudent(userContext.CurrentUser.StudentId));
            }
        }
    }
}