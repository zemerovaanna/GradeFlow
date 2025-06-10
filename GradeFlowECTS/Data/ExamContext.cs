using GradeFlowECTS.Interfaces;

namespace GradeFlowECTS.Data
{
    public class ExamContext : IExamContext
    {
        private Guid _examId;

        public Guid CurrentExamId => _examId;

        public void SetExamId(Guid examId)
        {
            _examId = examId;
        }

        public void ClearExamId()
        {
            _examId = Guid.Empty;
        }
    }
}