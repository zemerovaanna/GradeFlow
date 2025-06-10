using System.Windows;
using GradeFlowECTS.Interfaces;

namespace GradeFlowECTS.Infrastructure
{
    public class MessageBoxService : IMessageBoxService
    {
        public MessageBoxResult ShowConfirmation(string message, string caption = "Подтверждение")
        {
            return MessageBox.Show(message, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
        }

        public void ShowError(string message, string caption = "Ошибка")
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowInformation(string message, string caption = "Информация")
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public MessageBoxResult ShowQuestion(string message, string caption = "Вопрос")
        {
            return MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public void ShowWarning(string message, string caption = "Предупреждение")
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}