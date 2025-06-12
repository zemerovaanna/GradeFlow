using System.Diagnostics;
using System.Windows;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using Microsoft.EntityFrameworkCore;

namespace GradeFlowECTS.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly GradeFlowContext _context;
        private readonly ICryptographyService _cryptoryptographyService;
        private readonly IConfigurationService _configurationService;

        public TopicRepository(GradeFlowContext context, ICryptographyService cryptoryptographyService, IConfigurationService configurationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cryptoryptographyService = cryptoryptographyService ?? throw new ArgumentNullException(nameof(cryptoryptographyService));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        }

        private void Save()
        {
            _context.SaveChanges();
        }

        public List<TopicsDiscipline> GetTopicsByDisciplineId(int disciplineId)
        {
            try
            {
                return _context.TopicsDisciplines
                    .AsNoTracking()
                    .Where(t => t.DisciplineId == disciplineId)
                    .Include(t => t.Discipline)
                    .ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetTopicsByDisciplineId] Ошибка: {ex.Message}");
                return new List<TopicsDiscipline>();
            }
        }

        public void AddTopic(TopicsDiscipline topic)
        {
            try
            {
                topic.TopicName = _cryptoryptographyService.Encrypt(topic.TopicName, _configurationService.GetEncryptionKey());
                _context.TopicsDisciplines.Add(topic);
                Save();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AddTopic] Ошибка: {ex.Message}");
            }
        }

        public void UpdateTopic(TopicsDiscipline topic)
        {
            try
            {
                TopicsDiscipline existingTopic = _context.TopicsDisciplines.Find(topic.TopicDisciplinesId);
                existingTopic.DisciplineId = topic.DisciplineId;
                existingTopic.TopicName = _cryptoryptographyService.Encrypt(topic.TopicName, _configurationService.GetEncryptionKey());
                Save();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UpdateTopic] Ошибка: {ex.Message}");
            }
        }

        public TopicsDiscipline? GetTopicById(int topicId)
        {
            try
            {
                TopicsDiscipline? topic = _context.TopicsDisciplines.Find(topicId);
                topic.TopicName = _cryptoryptographyService.Decrypt(topic.TopicName, _configurationService.GetEncryptionKey());
                return topic;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetTopicById] Ошибка: {ex.Message}");
                return null;
            }
        }

        public void RemoveTopic(int topicId)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("ВСЁ СВЯЗАННОЕ С ТЕМОЙ БУДЕТ УДАЛЕНО", "АХТУНГ!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {

                    TopicsDiscipline existingTopic = _context.TopicsDisciplines.Find(topicId);

                    List<TopicsExamTest> topicsExamTestsList = _context.TopicsExamTests.Where(t => t.TopicId == existingTopic.TopicDisciplinesId).ToList();
                    List<Question> questionsList = _context.Questions.Where(q => q.TopicId == existingTopic.TopicDisciplinesId).ToList();

                    _context.TopicsExamTests.RemoveRange(topicsExamTestsList);
                    _context.Questions.RemoveRange(questionsList);
                    _context.TopicsDisciplines.Remove(existingTopic);
                    Save();
                }
                else
                {
                    MessageBox.Show("Ну и пошел нахрен тогда", "Ц!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RemoveTopic] Ошибка: {ex.Message}");
            }
        }
    }
}