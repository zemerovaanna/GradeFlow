using System.Windows;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.Services;
using GradeFlowECTS.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.View.Windows
{
    public partial class EditTopicWindow : Window
    {
        public EditTopicWindow(TopicsDiscipline topic)
        {
            InitializeComponent();

            GradeFlowContext context = App.Current.ServiceProvider.GetRequiredService<GradeFlowContext>();
            AesGcmCryptographyService cryptographyService = new AesGcmCryptographyService();
            FileService fileService = new FileService("GradeFlow");
            UserSettingsService userSettingsService = new UserSettingsService(fileService);
            ConfigurationService configurationService = new ConfigurationService(cryptographyService, fileService, userSettingsService);
            TopicRepository topicRepository = new TopicRepository(context, cryptographyService, configurationService);
            DisciplineRepository disciplineRepository = new DisciplineRepository(context, cryptographyService, configurationService);

            EditTopicViewModel editTopicViewModel = new EditTopicViewModel(topicRepository, disciplineRepository, topic);

            editTopicViewModel.SetRefreshAction = () => this.DialogResult = true;
            editTopicViewModel.CancelAction = () => this.Close();

            DataContext = editTopicViewModel;
        }
    }
}