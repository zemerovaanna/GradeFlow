using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GradeFlowECTS.Core;
using GradeFlowECTS.Helpers;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.View.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.ViewModel
{
    public class MailVerificationViewModel : BaseViewModel
    {
        private static readonly Brush ErrorBrush = Brushes.Red;
        private static readonly Brush DefaultBrush = Brushes.Black;

        private readonly IMailSender _mailSender;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWindowService _windowService;

        private string _code = string.Empty;
        private string _mail = "1zemerovaanna@gmail.com";
        private string _errorMessage = string.Empty;
        private Brush _codeBorderBrush;

        public event Action<string>? PasswordGenerated;
        public event Action NavigateToPasswordReset;

        public string Code
        {
            get => _code;
            set
            {
                if (_code != value)
                {
                    _code = value;
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            private set => SetProperty(ref _errorMessage, value);
        }

        public Brush CodeBorderBrush
        {
            get => _codeBorderBrush;
            set => SetProperty(ref _codeBorderBrush, value);
        }

        public ICommand ConfirmCommand { get; }
        public ICommand ResendCommand { get; }

        public MailVerificationViewModel(IMailSender mailSender, IServiceProvider serviceProvider, IWindowService windowService)
        {
            _mailSender = mailSender ?? throw new ArgumentNullException(nameof(mailSender));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));

            CodeBorderBrush = DefaultBrush;
            Code = string.Empty;

            ConfirmCommand = new RelayCommand(ConfirmCode);
            ResendCommand = new RelayCommand(ResendCode);

            ResendCode("null");
        }

        public void OnNavigateToPasswordReset()
        {
            NavigateToPasswordReset?.Invoke();
        }

        private void ConfirmCode(object parameter)
        {
            CodeBorderBrush = DefaultBrush;
            ErrorMessage = string.Empty;
            if (SecurityHelper.VerifyCode(_mail, Code!.ToUpper()))
            {
                _windowService.CloseWindow<InterludeWindow>();
                var windowManager = _serviceProvider.GetRequiredService<IWindowManager>();
                windowManager.ShowWindow<InterludeViewModel, PasswordResetViewModel>();
            }
            else
            {
                ErrorMessage = "Неверный код";
                CodeBorderBrush = ErrorBrush;
            }
        }

        private void ResendCode(object parameter)
        {
            SendCodeAsync();
        }

        private void SendCodeAsync()
        {
            try
            {
                string verificationCode = SecurityHelper.GenerateCode();
                SecurityHelper.StoreCode(_mail, verificationCode);

                _mailSender.SendVerificationCode(_mail, verificationCode, "Код для сброса пароля", "Ваш код: ");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{ex}";
            }
        }
    }
}