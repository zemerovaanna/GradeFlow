using GradeFlowECTS.Models;

namespace GradeFlowECTS.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с темами дисциплин.
    /// </summary>
    public interface ITopicRepository
    {
        /// <summary>
        /// Возвращает список тем по указанному идентификатору дисциплины.
        /// </summary>
        /// <param name="disciplineId">Иднтификатор дисциплины.</param>
        /// <returns>Список объектов типа TopicsDiscipline.</returns>
        List<TopicsDiscipline>GetTopicsByDisciplineId(int disciplineId);

        /// <summary>
        /// Добавление новой темы в базу данных.
        /// </summary>
        /// <param name="topic">Объект TopicsDiscipline.</param>
        void AddTopic(TopicsDiscipline topic);

        /// <summary>
        /// Обновляет данные существующей темы.
        /// </summary>
        /// <param name="topic">Объект TopicsDiscipline.</param>
        void UpdateTopic(TopicsDiscipline topic);

        /// <summary>
        /// Удаляет тему по указанному идентификатору.
        /// </summary>
        /// <param name="topicId">Идентификатор темы.</param>
        void RemoveTopic(int topicId);

        /// <summary>
        /// Возвращает тему по указанному идентификатору.
        /// </summary>
        /// <param name="topicId">Идентификатор темы.</param>
        TopicsDiscipline? GetTopicById(int topicId);
    }
}