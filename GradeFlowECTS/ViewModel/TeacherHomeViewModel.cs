using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GradeFlowECTS.Core;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.ViewModel.Items;

namespace GradeFlowECTS.ViewModel
{
    public class TeacherHomeViewModel : BaseViewModel
    {
        private readonly IExamContext _examContext;
        private readonly IExamRepository _examRepository;
        private readonly IMessageBoxService _messageBoxService;
        private readonly INavigationService _navigationService;
        private ObservableCollection<ExamItemViewModel> _exams;

        public ObservableCollection<ExamItemViewModel> Exams
        {
            get => _exams;
            set => SetProperty(ref _exams, value);
        }

        public Action<Exam>? EditExamAction { get; set; }
        public Action? GoExamAction { get; set; }

        public ICommand AddExamCommand { get; }
        public ICommand EditExamCommand { get; }
        public ICommand ExitAccountCommand { get; }
        public ICommand GoExamCommand { get; }
        public ICommand QuestionManagementCommand { get; }
        public ICommand RemoveExamCommand { get; }
        public ICommand TopicManagementCommand { get; }
        public ICommand UserManagementCommand { get; }

        public TeacherHomeViewModel(IExamContext examContext, IExamRepository examRepository, IMessageBoxService messageBoxService,INavigationService navigationService)
        {
            _examContext = examContext ?? throw new ArgumentNullException(nameof(examContext));
            _examRepository = examRepository ?? throw new ArgumentNullException(nameof(examRepository));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            AddExamCommand = new RelayCommand(AddExam);
            EditExamCommand = new RelayCommand(EditExam);
            ExitAccountCommand = new RelayCommand(ExitAccount);
            GoExamCommand = new RelayCommand(GoExam);
            QuestionManagementCommand = new RelayCommand(QuestionManagement);
            RemoveExamCommand = new RelayCommand(RemoveExam);
            TopicManagementCommand = new RelayCommand(TopicManagement);
            UserManagementCommand = new RelayCommand(UserManagement);

            RefreshExams();
        }

        public override void Dispose()
        {
            _navigationService.CurrentViewModelChanged -= OnCurrentViewModelChanged;
            base.Dispose();
        }

        private void AddExam(object? parameter) => _navigationService.NavigateTo<AddExamViewModel>();

        private void EditExam(object? parameter)
        {
            if (parameter is ExamItemViewModel examViewModel)
            {
                Exam? exam = _examRepository.GetExamById(examViewModel.ExamId);
                if (exam != null)
                {
                    EditExamAction?.Invoke(exam);
                }
            }
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
                    GoExamAction?.Invoke();
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

        private void QuestionManagement(object? parameter) => _navigationService.NavigateTo<QuestionManagementViewModel>();

        public void RefreshExams() => Exams = new ObservableCollection<ExamItemViewModel>(_examRepository.GetAllExamsWithGroup());

        private void RemoveExam(object? parameter)
        {
            if (parameter is ExamItemViewModel exam)
            {
                MessageBoxResult result = _messageBoxService.ShowConfirmation("Вы уверены, что хотите удалить элемент?");
                if (result == MessageBoxResult.Yes)
                {
                    _examRepository.RemoveExam(exam.ExamId);
                    Exams.Remove(exam);
                }
            }
        }

        private void TopicManagement(object? parameter) => _navigationService.NavigateTo<TopicManagementViewModel>();

        private void UserManagement(object? parameter) => _navigationService.NavigateTo<UserManagementViewModel>();
    }
}