using GradeFlowECTS.Models;

namespace GradeFlowECTS.Interfaces
{
    public interface IQuestionRepository
    {
        /// <summary>
        /// Возращает список вопрсов по указанному идентификатору темы.
        /// </summary>
        /// <param name="topicId">Идентификатор темы.</param>
        /// <returns>Список объектов типа Question.</returns>
        List<Question> GetAllQuestionsByTopicId(int topicId);

        /// <summary>
        /// Удаляет вопрос по указанному идентификатору.
        /// </summary>
        /// <param name="questionId">Идентификатор вопроса.</param>
        void RemoveQuestionById(int questionId);

        /// <summary>
        /// Добавление нового вопроса в базу данных.
        /// </summary>
        /// <param name="question">Объект типа Question.</param>
        void AddQuestion(Question question);
    }
}