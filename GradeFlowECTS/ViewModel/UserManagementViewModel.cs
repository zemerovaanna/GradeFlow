using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GradeFlowECTS.Core;
using GradeFlowECTS.Data;
using GradeFlowECTS.Helpers;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.View.Windows;
using GradeFlowECTS.ViewModel.Items;

namespace GradeFlowECTS.ViewModel
{
    public class UserManagementViewModel : BaseViewModel
    {
        private readonly object _filterLock = new object();

        private readonly IGroupRepository _groupRepository;
        private readonly IFileDialogService _fileDialogService;
        private readonly IMessageBoxService _messageBoxService;
        private readonly INavigationService _navigationService;
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IWindowService _windowService;

        private ObservableCollection<UserItemViewModel> _allUsers = new ObservableCollection<UserItemViewModel>();
        private ObservableCollection<UserRole> _roles = new ObservableCollection<UserRole>();
        private ICollectionView _userListView;
        private string _searchText;
        private UserRole _selectedRole;
        private StatusOption _selectedStatus;

        public ObservableCollection<StatusOption> StatusOptions { get; } = new ObservableCollection<StatusOption>
        {
            new StatusOption { Id = 0, DisplayName = "Все статусы", StatusValue = null },
            new StatusOption { Id = 1, DisplayName = "Активные", StatusValue = true },
            new StatusOption { Id = 2, DisplayName = "Неактивные", StatusValue = false }
        };

        public ObservableCollection<UserItemViewModel> AllUsers
        {
            get => _allUsers;
            private set
            {
                if (Equals(_allUsers, value)) return;

                _allUsers.Clear();
                foreach (var user in value)
                {
                    _allUsers.Add(user);
                }

                OnPropertyChanged(nameof(AllUsers));
                ApplyFilter();
            }
        }

        public ObservableCollection<UserRole> Roles
        {
            get => _roles;
            private set
            {
                _roles.Clear();
                foreach (var role in value)
                {
                    _roles.Add(role);
                }
                OnPropertyChanged(nameof(Roles));
            }
        }

        public ICollectionView UserListView
        {
            get => _userListView;
            private set
            {
                if (_userListView == value) return;

                _userListView = value;
                OnPropertyChanged(nameof(UserListView));
                ApplyFilter();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                ApplyFilter();
            }
        }

        public UserRole SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (_selectedRole == value) return;
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
                ApplyFilter();
            }
        }

        public StatusOption SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus == value) return;
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
                ApplyFilter();
            }
        }

        public Action<List<string>> GoExistingMailsAction { get; set; }

        public ICommand AddStudentCommand { get; }
        public ICommand LoadStudentsCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand RemoveUserCommand { get; }

        public UserManagementViewModel(IFileDialogService fileDialogService, IMessageBoxService messageBoxService, INavigationService navigationService, IUserContext userContext, IUserRepository userRepository, IUserRoleRepository userRoleRepository, IWindowService windowService, IGroupRepository groupRepository)
        {
            _fileDialogService = fileDialogService ?? throw new ArgumentNullException(nameof(fileDialogService));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userRoleRepository = userRoleRepository ?? throw new ArgumentNullException(nameof(userRoleRepository));
            _windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            _groupRepository = groupRepository;

            SelectedStatus = StatusOptions.First();

            _userListView = CollectionViewSource.GetDefaultView(_allUsers);
            _userListView.SortDescriptions.Add(new SortDescription("FullName", ListSortDirection.Ascending));

            AddStudentCommand = new RelayCommand(AddStudent);
            LoadStudentsCommand = new RelayCommand(LoadStudents);
            CancelCommand = new RelayCommand(_ => Cancel());
            RemoveUserCommand = new AsyncRelayCommand(RemoveUser);

            LoadData();

        }

        public class AsyncRelayCommand : ICommand
        {
            private readonly Func<object?, Task> _execute;
            private readonly Predicate<object?>? _canExecute;

            public AsyncRelayCommand(Func<object?, Task> execute, Predicate<object?>? canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

            public async void Execute(object? parameter)
            {
                await _execute(parameter); // вот здесь async
            }

            public event EventHandler? CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }
        }


        private async Task RemoveUser(object? parameter)
        {
            if (parameter is UserItemViewModel user)
            {
                var result = MessageBox.Show(
                    $"Вы действительно хотите удалить пользователя {user.LastName} {user.FirstName}?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    bool deleted = await _userRepository.DeleteUserAsync(user.UserId);

                    if (deleted)
                    {
                        var itemToRemove = _allUsers.FirstOrDefault(u => u.UserId == user.UserId);
                        if (itemToRemove != null)
                        {
                            _allUsers.Remove(itemToRemove);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить пользователя. Возможно, он не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show(parameter.ToString());
            }
        }

        private void LoadData()
        {
            try
            {
                LoadRoles();
                LoadUsers();
            }
            catch (Exception ex)
            {
                _messageBoxService.ShowError($"Ошибка загрузки данных: {ex.Message}", "Ошибка");
            }
        }

        public void ApplyFilter()
        {
            if (UserListView == null) return;

            string searchText = SearchText?.Trim().ToLowerInvariant();
            int selectedRoleId = SelectedRole?.RoleId ?? 0;
            bool? selectedStatus = SelectedStatus?.StatusValue;

            lock (_filterLock)
            {
                using (UserListView.DeferRefresh())
                {
                    Predicate<object> filter = item =>
                    {
                        if (item is UserItemViewModel user)
                        {
                            bool roleMatch = selectedRoleId == 0 || user.Role.RoleId == selectedRoleId;
                            bool statusMatch = !selectedStatus.HasValue || user.Status == selectedStatus.Value;
                            bool textMatch = string.IsNullOrEmpty(searchText) ||
                                           (user.FullName?.ToLowerInvariant().Contains(searchText) ?? false) ||
                                           (user.Mail?.ToLowerInvariant().Contains(searchText) ?? false);

                            return roleMatch && statusMatch && textMatch;
                        }
                        return false;
                    };

                    UserListView.Filter = filter;
                }
            }
        }

        private void LoadRoles()
        {
            try
            {
                List<UserRole> decryptedRoles = _userRoleRepository.GetAllRoles();
                var sortedRoles = decryptedRoles.OrderBy(r => r.RoleName).ToList();
                var allRolesOption = new UserRole { RoleId = 0, RoleName = "Все роли" };
                sortedRoles.Insert(0, allRolesOption);

                Roles = new ObservableCollection<UserRole>(sortedRoles);
                SelectedRole = Roles.FirstOrDefault();
            }
            catch (Exception ex)
            {

                _messageBoxService.ShowError($"Ошибка загрузки ролей: {ex.Message}", "Ошибка");
                Roles = new ObservableCollection<UserRole>();
            }
        }

        private void LoadUsers()
        {
            try
            {

                AllUsers.Clear();
                AllUsers = new ObservableCollection<UserItemViewModel>(_userRepository.GetAllUserItems().Where(u => u.UserId != _userContext.CurrentUser.UserId));
            }
            catch (Exception ex)
            {

                _messageBoxService.ShowError($"Ошибка загрузки пользователей: {ex.Message}", "Ошибка");
                AllUsers = new ObservableCollection<UserItemViewModel>();
            }
        }

        private void LoadStudents(object parameter)
        {
            // Получаем группы и оборачиваем в VM
            List<GroupItemViewModel> groupViewModels = _groupRepository.GetAllGroups();

            // Показываем выбор группы
            var groupSelector = new GroupSelectionWindow(groupViewModels);
            bool? result = groupSelector.ShowDialog();
            if (result != true || groupSelector.SelectedGroupVM == null)
                return;

            // Используем выбранную группу
            Group selectedGroup = groupSelector.SelectedGroupVM.Group;

            string filePath = _fileDialogService.OpenFileDialog(
                filter: "Text and CSV Files|*.txt;*.csv",
                initialDirectory: Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                multiselect: false);

            if (filePath == null) return;

            try
            {
                List<User> parsedStudents = ParseUsersFromFile(filePath);
                List<User> existingMails = new List<User>();
                List<User> newStudents = new List<User>();

                foreach (User student in parsedStudents)
                {
                    if (_userRepository.DoesEmailExist(student.Mail))
                    {
                        existingMails.Add(_userRepository.GetUserByEmail(student.Mail));
                    }
                    else
                    {
                        newStudents.Add(student);
                    }
                }

                if (existingMails.Any())
                {
                    HandleDuplicateMails(existingMails);
                }
                else
                {
                    AddStudentsToGroup(newStudents, selectedGroup);
                    _messageBoxService.ShowInformation(
                        $"Зарегистрировано количество студентов: {newStudents.Count}.",
                        "Файл обработан");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                _messageBoxService.ShowError($"Ошибка обработки файла: {ex.Message}", "Ошибка");
            }
        }

        private void AddStudentsToGroup(List<User> newUsers, Group group)
        {
            foreach (var user in newUsers)
            {
                var user2 = _userRepository.CreateNewUser(user.FirstName, user.MiddleName, user.LastName, user.Mail, user.Password, user.RoleId, user.Status);

                var student = new Student
                {
                    User = user2,
                    GroupId = group.GroupId
                };

                _userRepository.AddStudent(student);
            }
        }


        private void AddStudent(object parameter)
        {
            _navigationService.NavigateTo<UserRegistrationViewModel>();
        }

        private void AddStudents(List<User> newStudents)
        {
            _userRepository.AddStudents(newStudents);
            LoadUsers();
        }

        private List<User> ParseUsersFromFile(string filePath)
        {
            var users = CsvParserService.ParseUsersFromCsv(filePath);

            if (!users.Any())
            {
                throw new Exception("Файл пуст или не содержит валидных записей");
            }

            return users;
        }

        private void HandleDuplicateMails(List<User> existingMails)
        {
            MessageBoxResult result = _messageBoxService.ShowQuestion($"Найдено {existingMails.Count} пользователей с уже зарегистрированной почтой. Посмотреть подробнее?", "Внимание");
            if (result == MessageBoxResult.Yes)
            {
                GoExistingMailsAction?.Invoke(_userRepository.GetDecrypted(existingMails.Select(u => u.Mail).ToList()));
            }
        }

        private void Cancel()
        {
            _windowService.CloseWindow<InterludeWindow>();
            _navigationService.NavigateTo<TeacherHomeViewModel>();
        }
    }
}