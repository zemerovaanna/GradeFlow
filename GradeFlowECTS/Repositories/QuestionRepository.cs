using System.Diagnostics;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using Microsoft.EntityFrameworkCore;

namespace GradeFlowECTS.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly GradeFlowContext _context;
        private readonly ICryptographyService _cryptoryptographyService;
        private readonly IConfigurationService _configurationService;

        public QuestionRepository(GradeFlowContext context, ICryptographyService cryptoryptographyService, IConfigurationService configurationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cryptoryptographyService = cryptoryptographyService ?? throw new ArgumentNullException(nameof(cryptoryptographyService));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        }

        public List<Question> GetAllQuestionsByTopicId(int topicId)
        {
            try
            {
                return _context.Questions
                .Where(q => q.TopicId == topicId)
                .AsNoTracking()
                .ToList()
                .Select(q =>
                {
                    q.QuestionText = _cryptoryptographyService.Decrypt(q.QuestionText, _configurationService.GetEncryptionKey());
                    return q;
                })
                .OrderBy(q => q.QuestionText)
                .ToList(); ;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetAllQuestionsByTopicId] Ошибка: {ex.Message}");
                return new List<Question>();
            }
        }

        public void RemoveQuestionById(int questionId)
        {
            try
            {
                Question? existingTQuestion = _context.Questions.Find(questionId);
                _context.Questions.Remove(existingTQuestion);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RemoveQuestionById] Ошибка: {ex.Message}");
            }
        }

        public void AddQuestion(Question question)
        {
            try
            {
                _context.Questions.Add(question);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AddQuestion] Ошибка: {ex.Message}");
            }
        }
    }
}