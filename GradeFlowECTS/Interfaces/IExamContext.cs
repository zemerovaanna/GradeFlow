namespace GradeFlowECTS.Interfaces
{
    public interface IExamContext
    {
        Guid CurrentExamId { get; }
        void SetExamId(Guid examId);
        void ClearExamId();
    }
}