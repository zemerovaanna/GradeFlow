using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GradeFlowECTS.Core;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.ViewModel.Items;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.ViewModel
{
    public class AddExamViewModel : BaseViewModel
    {
        private static readonly Brush ErrorBrush = Brushes.Red;
        private static readonly Brush DefaultBrush = Brushes.Black;

        private readonly IDisciplineRepository _disciplineRepository;
        private readonly IExamRepository _examRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupsExamRepository _groupsExamRepository;
        private readonly IUserContext _userContext;
        private readonly INavigationService _navigationService;

        private string _examName;
        private string _hours;
        private string _minutes;
        private TimeOnly? _time;
        private DateTime _selectedDate = DateTime.Now;
        private Brush _examNameBorderBrush;
        private Brush _timeBorderBrush;
        private Discipline _selectedDiscipline;
        private ObservableCollection<GroupItemViewModel> _selectedGroups = new ObservableCollection<GroupItemViewModel>();

        public ObservableCollection<Discipline> Disciplines { get; } = new ObservableCollection<Discipline>();
        public ObservableCollection<GroupItemViewModel> AvailableGroups { get; } = new ObservableCollection<GroupItemViewModel>();
        public ObservableCollection<GroupItemViewModel> AllGroups { get; } = new ObservableCollection<GroupItemViewModel>();
        public ObservableCollection<GroupItemViewModel> SelectedGroups
        {
            get => _selectedGroups;
            set => SetProperty(ref _selectedGroups, value);
        }

        public Discipline SelectedDiscipline
        {
            get => _selectedDiscipline;
            set
            {
                if (SetProperty(ref _selectedDiscipline, value))
                {
                    UpdateAvailableGroups();
                }
            }
        }

        public string ExamName
        {
            get => _examName;
            set => SetProperty(ref _examName, value);
        }

        public string Hours
        {
            get => _hours;
            set
            {
                _hours = value;
                OnPropertyChanged();
                UpdateTime();
            }
        }

        public string Minutes
        {
            get => _minutes;
            set
            {
                _minutes = value;
                OnPropertyChanged();
                UpdateTime();
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        public Brush ExamNameBorderBrush
        {
            get => _examNameBorderBrush;
            set => SetProperty(ref _examNameBorderBrush, value);
        }

        public Brush TimeBorderBrush
        {
            get => _timeBorderBrush;
            set => SetProperty(ref _timeBorderBrush, value);
        }

        public TimeOnly? Time
        {
            get => _time;
            private set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public Visibility IsSelectedGroupsEmpty => SelectedGroups.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

        public ICommand CancelCommand { get; }
        public ICommand SaveExamCommand { get; }

        public AddExamViewModel(IDisciplineRepository disciplineRepository, IExamRepository examRepository, IGroupRepository groupRepository, IGroupsExamRepository groupsExamRepository, IUserContext userContext, INavigationService navigationService)
        {
            _disciplineRepository = disciplineRepository ?? throw new ArgumentNullException(nameof(disciplineRepository));
            _examRepository = examRepository ?? throw new ArgumentNullException(nameof(examRepository));
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _groupsExamRepository = groupsExamRepository ?? throw new ArgumentNullException(nameof(groupsExamRepository));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            Disciplines = new ObservableCollection<Discipline>(_disciplineRepository.GetAllDisciplines());
            AllGroups = new ObservableCollection<GroupItemViewModel>(_groupRepository.GetAllGroups());

            SelectedDiscipline = Disciplines.FirstOrDefault();

            SubscribeToSelectedGroupsChanges();

            Hours = "10";
            Minutes = "00";
            ExamNameBorderBrush = DefaultBrush;
            TimeBorderBrush = DefaultBrush;

            CancelCommand = new RelayCommand(Cancel);
            SaveExamCommand = new RelayCommand(Save);
        }

        private void Cancel(object? parameter) => _navigationService.NavigateTo<TeacherHomeViewModel>();

        private void Save(object? parameter)
        {
            ExamNameBorderBrush = DefaultBrush;
            if (string.IsNullOrWhiteSpace(ExamName))
            {
                ExamNameBorderBrush = ErrorBrush;
                return;
            }
            else if (Time == null || SelectedDiscipline == null || SelectedGroups.Count == 0)
            {
                return;
            }

            Exam exam = new Exam
            {
                ExamId = Guid.NewGuid(),
                DisciplineId = SelectedDiscipline.DisciplineId,
                ExamName = ExamName.Trim(),
                OpenDate = DateOnly.FromDateTime(SelectedDate),
                OpenTime = Time,
                OwnerTeacherId = _userContext.CurrentUser.TeacherId ?? 1,
                //PracticeTimeToComplete = 60
            };

            _examRepository.AddExam(exam);
            _groupsExamRepository.AddGroupsExam(SelectedGroups.ToList(), exam.ExamId);
            var context = App.Current.ServiceProvider.GetRequiredService<GradeFlowContext>();
            context.ExamPractices.Add(new ExamPractice { ExamId = exam.ExamId, PracticeTimeToComplete = 40 });
            _navigationService.NavigateTo<TeacherHomeViewModel>();
        }

        private void UpdateTime()
        {
            if (int.TryParse(Hours, out var h) && h is >= 0 and <= 23 &&
                int.TryParse(Minutes, out var m) && m is >= 0 and <= 59)
            {
                Time = new TimeOnly(h, m);
                TimeBorderBrush = DefaultBrush;
            }
            else
            {
                Time = null;
                TimeBorderBrush = ErrorBrush;
            }
        }

        private void SubscribeToSelectedGroupsChanges()
        {
            SelectedGroups.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(IsSelectedGroupsEmpty));
            };
        }

        private void UpdateAvailableGroups()
        {
            AvailableGroups.Clear();
            SelectedGroups.Clear();

            if (SelectedDiscipline == null) return;

            byte courseNumber = SelectedDiscipline.DisciplineName == "Квалификационный экзамен" ? (byte)3 : (byte)2;

            var groupsForCourse = AllGroups.Where(g => g.CourseNumber == courseNumber).ToList();

            foreach (var group in groupsForCourse)
            {
                AvailableGroups.Add(group);
            }
        }
    }
}