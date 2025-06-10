using GradeFlowECTS.Models;
using GradeFlowECTS.ViewModel.Items;

namespace GradeFlowECTS.Interfaces
{
    public interface IExamRepository
    {
        /// <summary>
        /// Добавляет данные об экзамене в базу данных.
        /// </summary>
        /// <param name="exam">Объект типа Exam.</param>
        void AddExam(Exam exam);

        /// <summary>
        /// Вовзращает список экзаменов с группами.
        /// </summary>
        /// <returns>Список объектов типа Exam.</returns>
        List<ExamItemViewModel> GetAllExamsWithGroup();

        /// <summary>
        /// Удаляет экзамен по указанному уникальному идентификатору.
        /// </summary>
        /// <param name="examId">Уникальный идентификатор экзамена.</param>
        /// <returns>Результат успешного удаления true; иначе false. </returns>
        bool RemoveExam(Guid examId);

        /// <summary>
        /// Возвращает экзамен по указанному уникальному идентификатору; иначе null.
        /// </summary>
        /// <param name="examId">Уникальный идентификатор.</param>
        /// <returns>Объект типа Exam.</returns>
        Exam? GetExamById(Guid examId);

        /// <summary>
        /// Обновляет данные об экзамене.
        /// </summary>
        /// <param name="exam">Объект экзамена с обновленными данными.</param>
        /// <returns>Результат успешного обновления true; иначе false.</returns>
        bool UpdateExam(Exam exam);

        /// <summary>
        /// Возвтращает логическое значение, существуют ли вопросы к экзамену.
        /// </summary>
        /// <param name="exam">Объект типа экзамен.</param>
        /// <returns>true если существует; иначе false.</returns>
        bool HasAnyQuestion(Exam exam);

        List<TopicsDiscipline> GetTopicsWithQuestionsByDisciplineId(int disciplineId);

        bool AddQuestionToExam(Guid examId, Question question);

        List<QuestionType> GetQuestionTypes();

        ExamTest CreateExamTest(Guid examId);

        void UpdateExamTest(ExamTest updatedExamTest);

        List<TopicsExamTest> GetSelectedTopicsByTestId(int examTestId);

        void SaveSelectedTopics(int examTestId, List<TopicWithQuestionsViewModel> selectedTopics);

        void SaveSelectedQuestions(int examTestId, List<Question> selectedQuestions);

        int RecalculateTotalPoints(int examTestId);

        ExamTest GetExamTestByExamId(Guid examId);

        string Decrypt(string encryptedString);

        List<ExamItemViewModel> GetUpcomingExamsForStudent(int? studentId);
    }
}