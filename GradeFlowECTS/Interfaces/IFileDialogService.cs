namespace GradeFlowECTS.Interfaces
{
    /// <summary>
    /// Сервис для открытия диалогового окна выбора файла.
    /// Позволяет настроить фильтр файлов, начальную директорию и режим мультивыбора.
    /// Возвращает путь к выбранному файлу или null, если выбор отменён.
    /// </summary>
    public interface IFileDialogService
    {
        /// <summary>
        /// Открывает диалоговое окно выбора файла с заданными параметрами.
        /// </summary>
        /// <param name="filter">Фильтр для отображаемых типов файлов.</param>
        /// <param name="initialDirectory">Начальная директория, которая откроется при запуске диалога.</param>
        /// <param name="multiselect">Разрешить ли выбор нескольких файлов.</param>
        /// <returns>Путь к выбранному файлу, если выбор подтверждён; иначе null.</returns>
        string OpenFileDialog(string filter, string initialDirectory, bool multiselect);
    }
}