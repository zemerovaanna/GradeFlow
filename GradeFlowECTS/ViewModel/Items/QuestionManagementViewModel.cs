using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GradeFlowECTS.Core;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;

namespace GradeFlowECTS.ViewModel.Items
{
    public class QuestionManagementViewModel : BaseViewModel
    {
        private readonly IDisciplineRepository _disciplineRepository;
        private readonly INavigationService _navigationService;
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;

        private ObservableCollection<Discipline> _disciplines;
        private ObservableCollection<Question> _questions;
        private ObservableCollection<TopicsDiscipline> _topics;
        private Discipline _selectedDiscipline;
        private TopicsDiscipline _selectedTopic;
        private string _questionText;

        public string QuestionText
        {
            get => _questionText;
            set => SetProperty(ref _questionText, value);
        }

        public ObservableCollection<Discipline> Disciplines
        {
            get => _disciplines;
            set => SetProperty(ref _disciplines, value);
        }

        public ObservableCollection<Question> Questions
        {
            get => _questions;
            set
            {
                SetProperty(ref _questions, value);
                OnPropertyChanged(nameof(Empty));
            }
        }

        public ObservableCollection<TopicsDiscipline> Topics
        {
            get => _topics;
            set => SetProperty(ref _topics, value);
        }

        public Discipline SelectedDiscipline
        {
            get => _selectedDiscipline;
            set
            {
                if (SetProperty(ref _selectedDiscipline, value))
                {
                    LoadTopicsForSelectedDiscipline();
                    if (Topics.Any())
                    {
                        SelectedTopic = Topics.First();
                    }
                    else
                    {
                        Questions.Clear();
                        OnPropertyChanged(nameof(Empty));
                    }
                }
            }
        }

        public TopicsDiscipline SelectedTopic
        {
            get => _selectedTopic;
            set
            {
                if (SetProperty(ref _selectedTopic, value))
                {
                    LoadQuestionsForSelectedTopic();
                }
            }
        }

        public Visibility Empty => Questions.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

        public Action<Question>? EditQuestionAction { get; set; }

        public ICommand AddQuestionCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand EditQuestionCommand { get; }
        public ICommand RemoveQuestionCommand { get; }

        public QuestionManagementViewModel(INavigationService navigationService,
                                           IDisciplineRepository disciplineRepository,
                                           IQuestionRepository questionRepository,
                                           ITopicRepository topicRepository)
        {
            _disciplineRepository = disciplineRepository ?? throw new ArgumentNullException(nameof(disciplineRepository));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
            _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));

            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;

            Disciplines = new ObservableCollection<Discipline>(_disciplineRepository.GetAllDisciplines());
            Questions = new ObservableCollection<Question>();
            Topics = new ObservableCollection<TopicsDiscipline>();

            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (Disciplines.Any())
                {
                    SelectedDiscipline = Disciplines.First();
                }
            });

            AddQuestionCommand = new RelayCommand(AddQuestion);
            BackCommand = new RelayCommand(Back);
            EditQuestionCommand = new RelayCommand(EditQuestion);
            RemoveQuestionCommand = new RelayCommand(RemoveQuestion);
        }

        private void LoadTopicsForSelectedDiscipline()
        {
            if (SelectedDiscipline != null)
            {
                var topics = _topicRepository.GetTopicsByDisciplineId(SelectedDiscipline.DisciplineId);
                Topics = new ObservableCollection<TopicsDiscipline>(topics);
                OnPropertyChanged(nameof(Topics));
            }
            else
            {
                Topics.Clear();
                Questions.Clear();
                OnPropertyChanged(nameof(Empty));
            }
        }

        private void LoadQuestionsForSelectedTopic()
        {
            if (SelectedTopic != null)
            {
                var questions = _questionRepository.GetAllQuestionsByTopicId(SelectedTopic.TopicDisciplinesId);
                Questions = new ObservableCollection<Question>(questions);
            }
            else
            {
                Questions.Clear();
            }

            OnPropertyChanged(nameof(Empty));
        }

        private void AddQuestion(object? parameter)
        {
            if (SelectedTopic == null)
            {
                MessageBox.Show("Пожалуйста, выберите тему для добавления вопроса.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(QuestionText))
            {
                MessageBox.Show("Текст вопроса не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newQuestion = new Question
            {
                TopicId = SelectedTopic.TopicDisciplinesId,
                QuestionText = QuestionText,
                QuestionTypeId = 1,
                ExamTestId = 1,
                FileName = "null",
                FileData = null
            };

            _questionRepository.AddQuestion(newQuestion);
            Questions.Add(newQuestion);
            QuestionText = string.Empty;

            OnPropertyChanged(nameof(Empty));

            MessageBox.Show("Вопрос успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Back(object? parameter)
        {
            _navigationService.NavigateTo<TeacherHomeViewModel>();
        }

        private void EditQuestion(object? parameter)
        {
            if (parameter is Question question)
            {
                EditQuestionAction?.Invoke(question);
            }
        }

        private void RemoveQuestion(object? parameter)
        {
            if (parameter is Question question)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить этот вопрос?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _questionRepository.RemoveQuestionById(question.QuestionId);
                    Questions.Remove(question);
                    OnPropertyChanged(nameof(Empty));
                }
            }
        }

        public override void Dispose()
        {
            _navigationService.CurrentViewModelChanged -= OnCurrentViewModelChanged;
            base.Dispose();
        }

        private void OnCurrentViewModelChanged()
        {
            if (_navigationService.CurrentViewModel is QuestionManagementViewModel)
            {
                Disciplines = new ObservableCollection<Discipline>(_disciplineRepository.GetAllDisciplines());

                if (SelectedDiscipline != null)
                {
                    LoadTopicsForSelectedDiscipline();
                    if (SelectedTopic != null)
                    {
                        LoadQuestionsForSelectedTopic();
                    }
                }
            }
        }
    }
}