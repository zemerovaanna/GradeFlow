using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GradeFlowECTS.Core;
using GradeFlowECTS.Helpers;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.ViewModel.Items;
using Microsoft.EntityFrameworkCore;

namespace GradeFlowECTS.ViewModel
{
    public class UserRegistrationViewModel : BaseViewModel
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMailSender _mailSender;
        private readonly IMessageBoxService _messageBoxService;
        private readonly INavigationService _navigationService;
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public event Action<string>? PasswordGenerated;
        new public event PropertyChangedEventHandler? PropertyChanged;

        private string _lastName = string.Empty;
        private string _firstName = string.Empty;
        private string _middleName = string.Empty;
        private string _mail = string.Empty;

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string MiddleName
        {
            get => _middleName;
            set => SetProperty(ref _middleName, value);
        }

        public string Mail
        {
            get => _mail;
            set => SetProperty(ref _mail, value);
        }

        private string _userPassword = string.Empty;
        private string _confirmPassword = string.Empty;

        public string UserPassword
        {
            get => _userPassword;
            set => SetProperty(ref _userPassword, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private ObservableCollection<UserRole> _roles;
        private UserRole _selectedRole;
        private ObservableCollection<GroupItemViewModel> _groups;
        private GroupItemViewModel? _selectedGroup;

        public ObservableCollection<UserRole> Roles
        {
            get => _roles;
            set => SetProperty(ref _roles, value);
        }

        public UserRole? SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (SetProperty(ref _selectedRole, value))
                {
                    UpdateVisibilityBasedOnRole();
                }
            }
        }


        public ObservableCollection<GroupItemViewModel> Groups
        {
            get => _groups;
            private set => SetProperty(ref _groups, value);
        }

        public GroupItemViewModel? SelectedGroup
        {
            get => _selectedGroup;
            set => SetProperty(ref _selectedGroup, value);
        }

        private Visibility _isRoleVisible;
        private Visibility _isGroupVisible;
        private Visibility _isTeacherCodeVisible;
        private Visibility _isCodeVerificationVisible = Visibility.Collapsed;
        private Visibility _isPasswordVisible = Visibility.Collapsed;

        public Visibility IsPasswordVisible
        {
            get => _isPasswordVisible;
            private set => SetProperty(ref _isPasswordVisible, value);
        }

        public Visibility IsRoleVisible
        {
            get => _isRoleVisible;
            private set => SetProperty(ref _isRoleVisible, value);
        }

        public Visibility IsGroupVisible
        {
            get => _isGroupVisible;
            private set => SetProperty(ref _isGroupVisible, value);
        }

        public Visibility IsTeacherCodeVisible
        {
            get => _isTeacherCodeVisible;
            set => SetProperty(ref _isTeacherCodeVisible, value);
        }

        public Visibility IsCodeVerificationVisible
        {
            get => _isCodeVerificationVisible;
            set => SetProperty(ref _isCodeVerificationVisible, value);
        }

        private string _teacherCode = string.Empty;
        private string _verificationCode = string.Empty;
        private string _errorMessage = string.Empty;

        public string TeacherCode
        {
            get => _teacherCode;
            private set => SetProperty(ref _teacherCode, value);
        }

        public string VerificationCode
        {
            get => _verificationCode;
            set => SetProperty(ref _verificationCode, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            private set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoadDataCommand { get; }
        public ICommand GeneratePasswordCommand { get; }
        public ICommand SendVerificationCodeCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand CancelCommand { get; }

        public UserRegistrationViewModel(IGroupRepository groupRepository, IMailSender mailSender, IMessageBoxService messageBoxService, INavigationService navigationService, IUserContext userContext, IUserRepository userRepository, IUserRoleRepository userRoleRepository)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _mailSender = mailSender ?? throw new ArgumentNullException(nameof(mailSender));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userRoleRepository = userRoleRepository ?? throw new ArgumentNullException(nameof(userRoleRepository));

            IsRoleVisible = _userContext.CurrentUser.TeacherId != null ? Visibility.Visible : Visibility.Collapsed;
            IsCodeVerificationVisible = _userContext.CurrentUser.TeacherId != null ? Visibility.Collapsed : Visibility.Visible;

            Roles = new ObservableCollection<UserRole>(_userRoleRepository.GetAllRoles());
            SelectedRole = Roles.FirstOrDefault();

            Groups = new ObservableCollection<GroupItemViewModel>(_groupRepository.GetAllGroups());
            SelectedGroup = Groups.FirstOrDefault();

            GeneratePasswordCommand = new RelayCommand(_ => GeneratePassword());
            SendVerificationCodeCommand = new RelayCommand(SendVerificationCodeAsync);
            RegisterCommand = new RelayCommand(ExecuteRegisterAsync);
            CancelCommand = new RelayCommand(_ => Navigate());
        }

        private void GeneratePassword()
        {
            string generatedPassword = SecurityHelper.GeneratePassword();
            UserPassword = generatedPassword;
            ConfirmPassword = generatedPassword;
            PasswordGenerated?.Invoke(generatedPassword);
        }

        private void SendVerificationCodeAsync(object p)
        {
            ClearErrorMessages();

            try
            {
                if (!InputValidator.IsValidMail(Mail, out var _))
                {
                    AddErrorMessage("Неверный формат почты.");
                    return;
                }

                if (_userRepository.DoesEmailExist(Mail.Trim()))
                {
                    AddErrorMessage("Пользователь с такой почтой уже существует.");
                    return;
                }

                string emailToSend = Mail.Trim();
                string verificationCode = SecurityHelper.GenerateCode();
                SecurityHelper.StoreCode(emailToSend, verificationCode);

                _mailSender.SendVerificationCode(emailToSend, verificationCode, "Код для подстверждения почты", "Ваш код подтверждения почты: ");
            }
            catch (Exception ex)
            {
                AddErrorMessage($"Ошибка отправки кода: {ex.Message}");
            }
        }

        private void ExecuteRegisterAsync(object? parameter)
        {
            ClearErrorMessages();

            string actualPassword = UserPassword ?? string.Empty;
            string actualConfirmPassword = ConfirmPassword ?? string.Empty;
            if (IsPasswordVisible == Visibility.Visible)
            {
                if (!ValidateRegistration(actualPassword, actualConfirmPassword))
                {
                    return;
                }
            }

            string lastName = InputValidator.TrimApostrophesAndRemoveSpaces(LastName);
            string firstName = InputValidator.TrimApostrophesAndRemoveSpaces(FirstName);
            string? middleName = string.IsNullOrWhiteSpace(MiddleName) ? null : InputValidator.TrimApostrophesAndRemoveSpaces(MiddleName);
            string email = InputValidator.TrimApostrophesAndRemoveSpaces(Mail);
            string verificationCode = string.IsNullOrWhiteSpace(VerificationCode) ? string.Empty : InputValidator.TrimApostrophesAndRemoveSpaces(VerificationCode);

            try
            {
                if (_userRepository.DoesFullNameExist(lastName, firstName, middleName))
                {
                    if (_messageBoxService.ShowQuestion("Пользователь с такими ФИО уже существует. Все равно продолжить регистрацию?", "Подтверждение") == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                if (_userRepository.DoesEmailExist(email))
                {
                    AddErrorMessage("Пользователь с такой почтой уже существует.");
                    return;
                }
            }
            catch (Exception ex)
            {
                AddErrorMessage($"Ошибка проверки существующих данных: {ex.Message}");
                return;
            }

            if (IsCodeVerificationVisible == Visibility.Visible)
            {
                if (string.IsNullOrWhiteSpace(verificationCode))
                {
                    AddErrorMessage("Для продолжения требуется код подтверждения почты.");
                    return;
                }

                if (!SecurityHelper.VerifyCode(email, verificationCode.ToUpper()))
                {
                    AddErrorMessage("Неверный код подтверждения. Проверьте почту или получите новый код.");
                    return;
                }
            }

            try
            {
                User newUser = _userRepository.CreateNewUser(lastName, firstName, middleName, email, actualPassword, SelectedRole.RoleId, true);

                if (newUser != null && _userRepository.DoesUserIdExist(newUser.UserId))
                {
                    bool assignmentSuccess = AssignRoleSpecificDataAsync(newUser);

                    if (assignmentSuccess)
                    {
                        _messageBoxService.ShowInformation("Регистрация прошла успешно.", "Сообщение");
                        ClearForm();
                        PasswordGenerated?.Invoke(string.Empty);
                        Navigate();
                    }
                    else
                    {
                        _messageBoxService.ShowError("Пользователь создан, но произошла ошибка при назначении деталей роли (группа/код). Обратитесь к администратору.", "Частичная ошибка");
                    }
                }
                else
                {
                    AddErrorMessage("Не удалось создать пользователя в базе данных.");
                    _messageBoxService.ShowError("Пользователь не зарегистрирован.", "Ошибка");
                }
            }
            catch (DbUpdateException dbEx)
            {
                AddErrorMessage($"Ошибка базы данных при регистрации: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                AddErrorMessage($"Непредвиденная ошибка регистрации: {ex.Message}");
            }
        }

        private bool AssignRoleSpecificDataAsync(User newUser)
        {
            try
            {
                switch (SelectedRole?.RoleName)
                {
                    case "Преподаватель":
                        if (IsTeacherCodeVisible == Visibility.Visible && !string.IsNullOrWhiteSpace(TeacherCode))
                        {
                            _userRepository.AssignTeacherWithCode(newUser.UserId, TeacherCode);
                            return true;
                        }
                        else
                        {
                            TeacherCode = "0000";
                            AddErrorMessage("Назначен код преподавателя: 0000.");
                            return false;
                        }

                    case "Студент":
                        if (IsGroupVisible == Visibility.Visible && SelectedGroup != null)
                        {
                            _userRepository.AssignStudentToGroup(newUser.UserId, SelectedGroup.GroupId);
                            return true;
                        }
                        else
                        {
                            AddErrorMessage("Группа для студента не выбрана или невидима.");
                            return false;
                        }

                    default:
                        return true;
                }
            }
            catch (Exception ex)
            {
                AddErrorMessage($"Ошибка при назначении деталей роли ({SelectedRole.RoleName}): {ex.Message}");
                return false;
            }
        }

        private void UpdateVisibilityBasedOnRole()
        {
            if (_userContext.CurrentUser.TeacherId == null)
            {
                // Пользователь — не преподаватель: жёстко задаём роль "Студент"
                SelectedRole = Roles.FirstOrDefault(r => r.RoleName == "Студент");
                IsRoleVisible = Visibility.Collapsed;
            }
            else
            {
                // Пользователь — преподаватель
                IsRoleVisible = Visibility.Visible;
            }

            // Визуальные элементы в зависимости от роли
            if (SelectedRole?.RoleName == "Студент")
            {
                IsGroupVisible = Visibility.Visible;
                IsTeacherCodeVisible = Visibility.Collapsed;
                IsPasswordVisible = Visibility.Collapsed;
            }
            else if (SelectedRole?.RoleName == "Преподаватель")
            {
                IsGroupVisible = Visibility.Collapsed;
                IsTeacherCodeVisible = Visibility.Visible;
                IsPasswordVisible = Visibility.Visible;
                GenerateTeacherCodeIfNeeded();
            }
            else
            {
                // На всякий случай
                IsGroupVisible = Visibility.Collapsed;
                IsTeacherCodeVisible = Visibility.Collapsed;
            }
        }


        private void GenerateTeacherCodeIfNeeded()
        {
            if (SelectedRole?.RoleName == "Преподаватель" && string.IsNullOrWhiteSpace(TeacherCode))
            {
                Random random = new Random();
                TeacherCode = random.Next(1000, 9999).ToString();
            }
        }

        private bool ValidateRegistration(string password, string confirmPassword)
        {
            bool isValid = true;
            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(LastName))
            {
                errorMessages.Add("Фамилия: обязательное поле.");
            }
            else if (!InputValidator.IsValidName(LastName, out string errorLastName))
            {
                errorMessages.Add(errorLastName);
            }


            if (string.IsNullOrWhiteSpace(FirstName))
            {
                errorMessages.Add("Имя: обязательное поле.");
            }
            else if (!InputValidator.IsValidName(FirstName, out string errorFirstName))
            {
                errorMessages.Add(errorFirstName);
            }


            if (!string.IsNullOrWhiteSpace(MiddleName) && !InputValidator.IsValidName(MiddleName, out string errorMiddleName))
            {
                errorMessages.Add(errorMiddleName);
            }


            if (string.IsNullOrWhiteSpace(Mail))
            {
                errorMessages.Add("Почта: обязательное поле.");
            }
            else if (!InputValidator.IsValidMail(Mail, out string errorMail))
            {
                errorMessages.Add(errorMail);
            }


            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessages.Add("Пароль: обязательное поле.");
            }
            else if (!InputValidator.IsValidPassword(password, out string errorPassword))
            {
                errorMessages.Add(errorPassword);
            }

            if (password != confirmPassword)
            {
                errorMessages.Add("Пароли не совпадают.");
            }

            if (IsRoleVisible == Visibility.Visible && SelectedRole == null)
            {
                errorMessages.Add("Роль: необходимо выбрать.");
            }

            if (IsTeacherCodeVisible == Visibility.Visible && string.IsNullOrWhiteSpace(TeacherCode))
            {
                errorMessages.Add("Код преподавателя: не сгенерирован или отсутствует (для роли Преподаватель).");
            }

            if (IsGroupVisible == Visibility.Visible && SelectedGroup == null)
            {
                errorMessages.Add("Группа: необходимо выбрать (для роли Студент).");
            }

            if (IsCodeVerificationVisible == Visibility.Visible && string.IsNullOrWhiteSpace(VerificationCode))
            {
                errorMessages.Add("Код подтверждения почты: обязателен для ввода.");
            }


            if (errorMessages.Any())
            {
                ErrorMessage = string.Join(Environment.NewLine, errorMessages);
                isValid = false;
            }

            return isValid;
        }

        private void ClearForm()
        {
            LastName = string.Empty;
            FirstName = string.Empty;
            MiddleName = string.Empty;
            Mail = string.Empty;
            VerificationCode = string.Empty;
            UserPassword = string.Empty;
            ConfirmPassword = string.Empty;
            ClearErrorMessages();
            PasswordGenerated?.Invoke(string.Empty);
        }

        private void AddErrorMessage(string message)
        {
            ErrorMessage += (string.IsNullOrEmpty(ErrorMessage) ? "" : Environment.NewLine) + message;
        }

        private void ClearErrorMessages()
        {
            ErrorMessage = string.Empty;
        }

        private void Navigate()
        {
            if (_userContext.CurrentUser.TeacherId != null)
            {
                _navigationService.NavigateTo<UserManagementViewModel>();
            }
            else
            {
                _navigationService.NavigateTo<StudentLoginViewModel>();
            }
        }
    }
}