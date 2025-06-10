namespace GradeFlowECTS.Data
{
    public class UserAnswer
    {
        public int QuestionId { get; set; }
        public HashSet<int> SelectedAnswerIds { get; set; } = new();
    }
}