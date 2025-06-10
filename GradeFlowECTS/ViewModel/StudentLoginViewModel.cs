using System.Collections.ObjectModel;
using System.Windows.Input;
using GradeFlowECTS.Core;
using GradeFlowECTS.Data;
using GradeFlowECTS.Helpers;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.View.Windows;
using GradeFlowECTS.ViewModel.Items;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace GradeFlowECTS.ViewModel
{
    public class StudentLoginViewModel : BaseViewModel
    {
        private readonly IGroupRepository _groupRepository;
        private readonly INavigationService _navigationService;
        private readonly IUserRepository _userRepository;
        private readonly IMailSender _mailSender;
        private readonly IServiceProvider _serviceProvider;

        private GroupItemViewModel _selectedGroup;
        public GroupItemViewModel SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                if (SetProperty(ref _selectedGroup, value))
                {
                    LoadStudentsForSelectedGroup();
                }
            }
        }

        private StudentItemViewModel _selectedStudent;
        public StudentItemViewModel SelectedStudent
        {
            get => _selectedStudent;
            set => SetProperty(ref _selectedStudent, value);
        }

        private ObservableCollection<GroupItemViewModel> _groups;
        public ObservableCollection<GroupItemViewModel> Groups
        {
            get => _groups;
            set => SetProperty(ref _groups, value);
        }

        private ObservableCollection<StudentItemViewModel> _students;
        public ObservableCollection<StudentItemViewModel> Students
        {
            get => _students;
            set => SetProperty(ref _students, value);
        }

        private string _verificationCode;
        public string VerificationCode
        {
            get => _verificationCode;
            set => SetProperty(ref _verificationCode, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }


        public ICommand SendStudentCodeCommand { get; }
        public ICommand LoginAsStudentCommand { get; }
        public ICommand NavigateToTeacherLoginCommand { get; }


        public StudentLoginViewModel(IGroupRepository groupRepository, INavigationService navigationService, IUserRepository userRepository, IMailSender mailSender, IServiceProvider serviceProvider)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mailSender = mailSender;
            _serviceProvider = serviceProvider;

            Groups = new ObservableCollection<GroupItemViewModel>(_groupRepository.GetAllGroups());
            SelectedGroup = Groups.FirstOrDefault();

            SendStudentCodeCommand = new RelayCommand(SendCodeAsync);
            LoginAsStudentCommand = new RelayCommand(LoginAsStudent);
            NavigateToTeacherLoginCommand = new RelayCommand(NavigateToTeacherLogin);

        }

        private void LoadStudentsForSelectedGroup()
        {
            if (SelectedGroup == null)
            {
                Students = new ObservableCollection<StudentItemViewModel>();
                return;
            }

            var students = _userRepository.GetStudentsByGroupId(SelectedGroup.GroupId);
            Students = new ObservableCollection<StudentItemViewModel>(students);
            SelectedStudent = Students.FirstOrDefault();
        }

        private void SendCodeAsync(object? parameter)
        {
            VerificationCode = string.Empty;
            if (SelectedStudent == null || string.IsNullOrWhiteSpace(SelectedStudent.UserItem.Mail))
            {
                // обработка ошибки.
                return;
            }

            try
            {
                string code = SecurityHelper.GenerateCode();
                SecurityHelper.StoreCode(SelectedStudent.UserItem.Mail, code);

                _mailSender.SendVerificationCode(
                    SelectedStudent.UserItem.Mail,
                    code,
                    "Код подтверждения входа",
                    "Ваш код для входа: "
                );
            }
            catch (Exception ex)
            {
                // Обработка ошибок (например, показать пользователю).
            }
        }

        private void LoginAsStudent(object? parameter)
        {
            try
            {
                if(Students.Count == 0 || VerificationCode.IsNullOrEmpty())
                {
                    return;
                }

                User user = _userRepository.GetUserByEmail(SelectedStudent.UserItem.Mail);
                if (user == null)
                {
                    ErrorMessage = "Пользователь не найден.";
                    return;
                }

                if (SecurityHelper.VerifyCode(SelectedStudent.UserItem.Mail, VerificationCode!.ToUpper()))
                {
                    var userContext = _serviceProvider.GetRequiredService<IUserContext>();
                    userContext.SetUser(new LocalUser { UserId = user.UserId, StudentId = _userRepository.GetStudentIdByuserGuid(user.UserId), Mail = user.Mail, RoleId = user.RoleId });
                    _navigationService.NavigateTo<StudentHomeViewModel>();
                }
                else
                {
                    ErrorMessage = "Неверный код";
                }
            }
            catch
            {
                ErrorMessage = "Пожалуйста, подождите.\n";
            }
        }

        private void NavigateToTeacherLogin(object? parameter)
        {
            _navigationService.NavigateTo<TeacherLoginViewModel>();
        }
    }
}