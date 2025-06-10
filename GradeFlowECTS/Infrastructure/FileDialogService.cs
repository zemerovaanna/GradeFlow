using GradeFlowECTS.Interfaces;
using Microsoft.Win32;

namespace GradeFlowECTS.Infrastructure
{
    public class FileDialogService : IFileDialogService
    {
        public string OpenFileDialog(string filter, string initialDirectory, bool multiselect)
        {
            var fileDialog = new OpenFileDialog()
            {
                Filter = filter,
                InitialDirectory = initialDirectory,
                Multiselect = multiselect
            };

            return fileDialog.ShowDialog() == true ? fileDialog.FileName : null;
        }
    }
}