namespace GradeFlowECTS.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string Mail { get; set; } = null!;

    public string? Password { get; set; }

    public int RoleId { get; set; }

    public bool Status { get; set; }

    public virtual UserRole Role { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
