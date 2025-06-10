namespace GradeFlowECTS.Data
{
    public class LocalUser
    {
        public Guid UserId { get; set; }
        public int? TeacherId { get; set; }
        public int? StudentId { get; set; }
        public string Mail { get; set; }
        public int RoleId { get; set; }
    }
}