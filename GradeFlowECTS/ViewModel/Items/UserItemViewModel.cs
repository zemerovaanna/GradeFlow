using GradeFlowECTS.Models;

namespace GradeFlowECTS.ViewModel.Items
{
    public class UserItemViewModel
    {
        public Guid UserId { get; set; }
        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string Mail { get; set; } = null!;

        public UserRole Role { get; set; }

        public string FullName => $"{LastName} {FirstName} {MiddleName}";
        public bool Status { get; set; }

        public UserItemViewModel() { }

        public UserItemViewModel(User user)
        {
            UserId = user.UserId;
            LastName = user.LastName;
            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            Mail = user.Mail;
            Role = user.Role;
            Status = user.Status;
        }
    }
}