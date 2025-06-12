using System.Collections.ObjectModel;

namespace GradeFlowECTS.Trash
{
    public class GroupViewModel
    {
        public int GroupNumber { get; set; }
        public ObservableCollection<StudentViewModel> Students { get; set; } = new();

        public GroupViewModel(int number)
        {
            GroupNumber = number;
        }
    }
}