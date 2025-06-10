using System.Windows;
using GradeFlowECTS.Data;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.Services;
using GradeFlowECTS.View.Windows;
using GradeFlowECTS.ViewModel;
using GradeFlowECTS.ViewModel.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; }
        public new static App Current => (App)Application.Current;

        public App()
        {
            ServiceProvider = ConfigureServices();
        }

        private static IServiceProvider ConfigureServices()
        {
            ServiceCollection services = new ServiceCollection();

            const string appName = "GradeFlow";

            services.AddDbContext<GradeFlowContext>((provider, options) =>
            {
                var configService = provider.GetRequiredService<IConfigurationService>();
                options.UseSqlServer(configService.GetConnectionString()).EnableSensitiveDataLogging();
            });

            services.AddScoped<INavigationService, NavigationService>();
            services.AddSingleton<IWindowService, WindowService>();
            services.AddSingleton<IWindowManager, WindowManager>();
            services.AddSingleton<IUserContext, UserContext>();

            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<ICryptographyService, AesGcmCryptographyService>();
            services.AddScoped<IDisciplineRepository, DisciplineRepository>();
            services.AddSingleton<IExamContext, ExamContext>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddSingleton<IFileDialogService, FileDialogService>();
            services.AddSingleton<IFileService>(_ => new FileService(appName));
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IGroupsExamRepository, GroupsExamRepository>();
            services.AddTransient<IMailSender, MailSender>();
            services.AddTransient<IMailSettingsService, MailSettingsService>();
            services.AddSingleton<IMessageBoxService, MessageBoxService>();
            services.AddTransient<IMimeMessageService, MimeMessageService>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddTransient<IUserSettingsService, UserSettingsService>();

            services.AddTransient<AddExamViewModel>();
            services.AddTransient<AddTopicViewModel>();
            services.AddTransient<DbConnectionViewModel>();
            services.AddTransient<ExamManagementViewModel>();
            services.AddTransient<GradeFlowViewModel>();
            services.AddTransient<InterludeViewModel>();
            services.AddTransient<MailVerificationViewModel>();
            services.AddTransient<PasswordResetViewModel>();
            services.AddTransient<QuestionManagementViewModel>();
            services.AddTransient<StudentExamsViewModel>();
            services.AddTransient<StudentGroupsManagementViewModel>();
            services.AddTransient<StudentHomeViewModel>();
            services.AddTransient<StudentLoginViewModel>();
            services.AddTransient<TeacherHomeViewModel>();
            services.AddTransient<TeacherLoginViewModel>();
            services.AddTransient<TopicManagementViewModel>();
            services.AddTransient<UserManagementViewModel>();
            services.AddTransient<UserRegistrationViewModel>();

            return services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            GradeFlowViewModel gradeFlowViewModel = ServiceProvider.GetRequiredService<GradeFlowViewModel>();
            GradeFlowWindow gradeFlowWindow = new GradeFlowWindow(gradeFlowViewModel);
            MainWindow = gradeFlowWindow;
            gradeFlowWindow.Show();
        }
    }
}