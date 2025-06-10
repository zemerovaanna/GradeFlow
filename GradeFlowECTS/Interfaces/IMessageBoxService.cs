using System.Windows;

namespace GradeFlowECTS.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для отображения диалоговых окон сообщений.
    /// Предоставляет методы для показа различных типов сообщений: подтверждения, ошибки,
    /// информации, вопроса и предупреждения.
    /// </summary>
    public interface IMessageBoxService
    {
        /// <summary>
        /// Отображает диалоговое окно подтверждения с указанным сообщением и заголовком.
        /// </summary>
        /// <param name="message">Текст сообщения для отображения.</param>
        /// <param name="caption">Заголовок окна (по умолчанию "Подтверждение").</param>
        /// <returns>Результат диалогового окна (<see cref="MessageBoxResult"/>).</returns>
        MessageBoxResult ShowConfirmation(string message, string caption = "Подтверждение");

        /// <summary>
        /// Отображает диалоговое окно ошибки с указанным сообщением и заголовком.
        /// </summary>
        /// <param name="message">Текст сообщения об ошибке.</param>
        /// <param name="caption">Заголовок окна (по умолчанию "Ошибка").</param>
        void ShowError(string message, string caption = "Ошибка");

        /// <summary>
        /// Отображает диалоговое окно информации с указанным сообщением и заголовком.
        /// </summary>
        /// <param name="message">Текст информационного сообщения.</param>
        /// <param name="caption">Заголовок окна (по умолчанию "Информация").</param>
        void ShowInformation(string message, string caption = "Информация");

        /// <summary>
        /// Отображает диалоговое окно вопроса с указанным сообщением и заголовком.
        /// </summary>
        /// <param name="message">Текст вопроса.</param>
        /// <param name="caption">Заголовок окна (по умолчанию "Вопрос").</param>
        /// <returns>Результат диалогового окна (<see cref="MessageBoxResult"/>).</returns>
        MessageBoxResult ShowQuestion(string message, string caption = "Вопрос");

        /// <summary>
        /// Отображает диалоговое окно предупреждения с указанным сообщением и заголовком.
        /// </summary>
        /// <param name="message">Текст предупреждения.</param>
        /// <param name="caption">Заголовок окна (по умолчанию "Предупреждение").</param>
        void ShowWarning(string message, string caption = "Предупреждение");
    }
}