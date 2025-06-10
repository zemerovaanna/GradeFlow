using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GradeFlowECTS.Core;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.ViewModel.Items;

namespace GradeFlowECTS.ViewModel
{
    public class EditExamViewModel : BaseViewModel
    {
        private static readonly Brush ErrorBrush = Brushes.Red;
        private static readonly Brush DefaultBrush = Brushes.Black;

        private readonly IDisciplineRepository _disciplineRepository;
        private readonly IExamRepository _examRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupsExamRepository _groupsExamRepository;

        private readonly Guid _examGuid;
        private string _examName;
        private string _hours;
        private string _minutes;
        private TimeOnly? _time;
        private DateTime _selectedDate = DateTime.Now;
        private int? _ownerId;
        private Brush _examNameBorderBrush;
        private Brush _timeBorderBrush;
        private Discipline _selectedDiscipline;
        private ObservableCollection<GroupItemViewModel> _selectedGroups = new ObservableCollection<GroupItemViewModel>();
        private ObservableCollection<Discipline> _disciplines;

        public ObservableCollection<Discipline> Disciplines
        {
            get => _disciplines;
            set => SetProperty(ref _disciplines, value);
        }

        public ObservableCollection<GroupItemViewModel> AvailableGroups { get; } = new ObservableCollection<GroupItemViewModel>();
        public ObservableCollection<GroupItemViewModel> AllGroups { get; } = new ObservableCollection<GroupItemViewModel>();

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

        public ObservableCollection<GroupItemViewModel> SelectedGroups
        {
            get => _selectedGroups;
            set
            {
                if (_selectedGroups != null)
                {
                    _selectedGroups.CollectionChanged -= SelectedGroups_CollectionChanged;
                }

                if (SetProperty(ref _selectedGroups, value))
                {
                    _selectedGroups.CollectionChanged += SelectedGroups_CollectionChanged;
                    OnPropertyChanged(nameof(IsSelectedGroupsEmpty));
                }
            }
        }

        private void SelectedGroups_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IsSelectedGroupsEmpty));
        }

        public Visibility IsSelectedGroupsEmpty => SelectedGroups.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        public Action SetRefreshAction { get; set; }
        public Action CancelAction { get; set; }
        public ICommand UpdateExamCommand { get; }
        public ICommand CancelCommand { get; }

        public EditExamViewModel(IDisciplineRepository disciplineRepository, IExamRepository examRepository, IGroupRepository groupRepository, IGroupsExamRepository groupsExamRepository, Exam exam)
        {
            _disciplineRepository = disciplineRepository ?? throw new ArgumentNullException(nameof(disciplineRepository));
            _examRepository = examRepository ?? throw new ArgumentNullException(nameof(examRepository));
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _groupsExamRepository = groupsExamRepository ?? throw new ArgumentNullException(nameof(groupsExamRepository));

            Disciplines = new ObservableCollection<Discipline>(_disciplineRepository.GetAllDisciplines());
            AllGroups = new ObservableCollection<GroupItemViewModel>(_groupRepository.GetAllGroups());

            _examGuid = exam.ExamId;
            _ownerId = exam.OwnerTeacherId;
            SelectedDiscipline = Disciplines.Where(d => d.DisciplineId == exam.DisciplineId).FirstOrDefault();

            ExamName = exam.ExamName;
            TimeOnly? time = exam.OpenTime;
            Hours = time?.Hour.ToString();
            Minutes = $"{time?.Minute:D2}";
            DateOnly? date = exam.OpenDate;
            if (date.HasValue)
            {
                SelectedDate = date.Value.ToDateTime(TimeOnly.MinValue);
            }
            var selectedGroupIds = _groupsExamRepository.GetGroupsExamById(exam.ExamId).Select(g => g.GroupId).ToHashSet();

            SelectedGroups = new ObservableCollection<GroupItemViewModel>(
                AllGroups.Where(g => selectedGroupIds.Contains(g.GroupId))
            );

            TimeBorderBrush = DefaultBrush;

            UpdateExamCommand = new RelayCommand(UpdateExam);
            CancelCommand = new RelayCommand(_ => CancelAction?.Invoke());
        }

        private void UpdateExam(object? parameter)
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
                ExamId = _examGuid,
                DisciplineId = SelectedDiscipline.DisciplineId,
                ExamName = ExamName.Trim(),
                OpenDate = DateOnly.FromDateTime(SelectedDate),
                OpenTime = Time,
                OwnerTeacherId = _ownerId ?? 1
            };

            _examRepository.UpdateExam(exam);

            _groupsExamRepository.UpdateGroupsExam(SelectedGroups.ToList(), exam.ExamId);
            SetRefreshAction.Invoke();
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

        private void UpdateAvailableGroups()
        {
            AvailableGroups.Clear();
            // Убираем это:
            // SelectedGroups.Clear();

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