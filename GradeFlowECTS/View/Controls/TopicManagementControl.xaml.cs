using System.Windows.Controls;
using GradeFlowECTS.View.Windows;
using GradeFlowECTS.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace GradeFlowECTS.View.Controls
{
    public partial class TopicManagementControl : UserControl
    {
        public TopicManagementControl()
        {
            InitializeComponent();

            TopicManagementViewModel topicManagementViewModel = App.Current.ServiceProvider.GetRequiredService<TopicManagementViewModel>();

            topicManagementViewModel.EditTopicAction = topic =>
            {
                EditTopicWindow editWindow = new EditTopicWindow(topic);
                if (editWindow.ShowDialog() == true)
                {
                    topicManagementViewModel.RefreshTopic();
                }
            };

            DataContext = topicManagementViewModel;
        }
    }
}