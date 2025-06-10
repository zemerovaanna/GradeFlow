using System.Windows.Input;
using System.Windows.Media;
using GradeFlowECTS.Core;
using GradeFlowECTS.Data;
using GradeFlowECTS.Helpers;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.ViewModel
{
    public class TeacherLoginViewModel : BaseViewModel
    {
        private enum FieldType { Mail, Password }
        private static readonly Brush ErrorBrush = Brushes.Red;
        private static readonly Brush DefaultBrush = Brushes.Black;

        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IWindowManager _windowManager;

        private string _errorMessage = string.Empty;
        private Brush _mailBorderBrush;
        private string _teacherMail = string.Empty;
        private string _teacherPassword = string.Empty;
        private Brush _passwordBorderBrush;

        public string ErrorMessage
        {
            get => _errorMessage;
            private set => SetProperty(ref _errorMessage, value);
        }

        public Brush MailBorderBrush
        {
            get => _mailBorderBrush;
            set => SetProperty(ref _mailBorderBrush, value);
        }

        public Brush PasswordBorderBrush
        {
            get => _passwordBorderBrush;
            set => SetProperty(ref _passwordBorderBrush, value);
        }

        public string TeacherMail
        {
            get => _teacherMail;
            set
            {
                if (SetProperty(ref _teacherMail, value))
                {
                    ErrorMessage = string.Empty;
                }
            }
        }

        public string TeacherPassword
        {
            get => _teacherPassword;
            set => SetProperty(ref _teacherPassword, value);
        }

        public ICommand LoginAsTeacherCommand { get; }
        public ICommand NavigateToStudentLoginCommand { get; }
        public ICommand OpenCodeVerificationCommand { get; }

        public event Action<string>? PasswordGenerated;

        public TeacherLoginViewModel(INavigationService navigationService, IServiceProvider serviceProvider,
            IUserRepository userRepository, IUserRoleRepository userRoleRepository, IWindowManager windowManager)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userRoleRepository = userRoleRepository ?? throw new ArgumentNullException(nameof(userRoleRepository));
            _windowManager = windowManager ?? throw new ArgumentNullException(nameof(windowManager));

            LoginAsTeacherCommand = new RelayCommand(Login);
            OpenCodeVerificationCommand = new RelayCommand(OpenCodeVerification);
            NavigateToStudentLoginCommand = new RelayCommand(NavigateToStudentLogin);

            _teacherMail = "1zemerovaanna@gmail.com";
            _teacherPassword = "*q5qGaP%iY%KbG";
            ResetBorders();
        }

        private void Login(object parameter)
        {
            ErrorMessage = string.Empty;
            ResetBorders();

            if (!ValidateExists(TeacherMail))
                return;

            if (string.IsNullOrWhiteSpace(TeacherPassword))
            {
                ErrorMessage = "Пожалуйста, введите пароль.\n";
                SetErrorBorder(FieldType.Password);
                return;
            }

            LoginAsTeacher(TeacherMail, TeacherPassword);
        }

        private void LoginAsTeacher(string mail, string password)
        {
            try
            {
                User user = _userRepository.GetUserByEmail(mail);
                if (user == null)
                {
                    ErrorMessage = "Пользователь не найден.";
                    SetErrorBorder(FieldType.Mail);
                    return;
                }

                string roleName = _userRoleRepository.GetRoleNameById(user.RoleId);
                if (roleName != "Преподаватель")
                {
                    ErrorMessage = "Вы не можете войти как преподаватель.";
                    return;
                }

                if (!PasswordHasher.VerifyPassword(password, user.Password!))
                {
                    ErrorMessage = "Неверный пароль.";
                    SetErrorBorder(FieldType.Password);
                    return;
                }

                if (roleName == "Преподаватель")
                {
                    var userContext = _serviceProvider.GetRequiredService<IUserContext>();
                    userContext.SetUser(new LocalUser { UserId = user.UserId, TeacherId = _userRepository.GetTeacherIdByuserGuid(user.UserId), Mail = user.Mail, RoleId = user.RoleId });
                    _navigationService.NavigateTo<TeacherHomeViewModel>();
                }
            }
            catch
            {
                ErrorMessage = "Пожалуйста, подождите.\n";
            }
        }

        private void NavigateToStudentLogin(object? parameter)
        {
            _navigationService.NavigateTo<StudentLoginViewModel>();
        }

        private void OpenCodeVerification(object parameter)
        {
            ErrorMessage = string.Empty;
            ResetBorders();

            if (!ValidateExists(_teacherMail))
            {
                return;
            }

            var userContext = _serviceProvider.GetRequiredService<IUserContext>();
            userContext.SetUser(new LocalUser { Mail = _teacherMail });
            _windowManager.ShowWindow<InterludeViewModel, MailVerificationViewModel>();
        }

        private void ResetBorders()
        {
            MailBorderBrush = DefaultBrush;
            PasswordBorderBrush = DefaultBrush;
        }

        private void SetErrorBorder(FieldType field)
        {
            switch (field)
            {
                case FieldType.Mail:
                    MailBorderBrush = ErrorBrush;
                    break;
                case FieldType.Password:
                    PasswordBorderBrush = ErrorBrush;
                    break;
            }
        }

        private bool ValidateExists(string mail)
        {
            if (string.IsNullOrWhiteSpace(mail))
            {
                ErrorMessage = "Пожалуйста, введите почту.";
                SetErrorBorder(FieldType.Mail);
                return false;
            }

            if (!InputValidator.IsValidMail(mail, out string mailError))
            {
                ErrorMessage = mailError;
                SetErrorBorder(FieldType.Mail);
                return false;
            }

            if (!_userRepository.DoesEmailExist(mail))
            {
                ErrorMessage = "Пользователя с такой почтой не существует.";
                SetErrorBorder(FieldType.Mail);
                return false;
            }

            return true;
        }
    }
}