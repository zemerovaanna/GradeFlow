using System.Collections.ObjectModel;
using System.ComponentModel;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Models;
using Microsoft.EntityFrameworkCore;

namespace GradeFlowECTS.StudentManagment
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected)); // <=== этого не хватает!
                }
            }
        }
        private bool _isSelected;


        public int StudentId { get; set; }
        public Guid UserId { get; set; }

        public string FullName => $"{User?.LastName} {User?.FirstName} {User?.MiddleName}";
        public string Email => User?.Mail;

        private GroupItemViewModel _selectedGroup;
        public GroupItemViewModel SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                if (_selectedGroup != value)
                {
                    _selectedGroup = value;
                    OnPropertyChanged(nameof(SelectedGroup));
                    SaveGroupChange();
                }
            }
        }

        public User User { get; set; }

        public ObservableCollection<GroupItemViewModel> AvailableGroups { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public StudentViewModel(Student student, ObservableCollection<GroupItemViewModel> availableGroups)
        {
            StudentId = student.StudentId;
            UserId = student.UserId;
            User = student.User;
            AvailableGroups = availableGroups;

            SelectedGroup = AvailableGroups.FirstOrDefault(g => g.GroupId == student.GroupId);
        }

        private void SaveGroupChange()
        {
            if (SelectedGroup == null)
                return;

            using var db = new GradeFlowContext();
            var student = db.Students.Include(s => s.Group).FirstOrDefault(s => s.StudentId == StudentId);
            if (student != null)
            {
                student.GroupId = SelectedGroup.GroupId;
                db.SaveChanges();
            }
        }

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}