using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media.TextFormatting;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.ViewModel.Items;
using Microsoft.EntityFrameworkCore;

namespace GradeFlowECTS.Repositories
{
    public class ExamRepository : IExamRepository
    {
        public void UpdateQuestion(Question question)
        {
            _context.Questions.Update(question);
            _context.SaveChanges();
        }

        static class LOL
        {
            private const int KeySize = 32;
            private const int IvSize = 12;
            private const int TagSize = 16;

            public static string Encrypt(string? plainText)
            {
                try
                {
                    if (plainText != null)
                    {
                        var key = "GradeFlowWPF" + ComplexComputation();
                        byte[] keyBytes = GetKey(key);
                        byte[] iv = new byte[IvSize];

                        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                        {
                            rng.GetBytes(iv);
                        }

                        using (AesGcm aes = new AesGcm(keyBytes, TagSize))
                        {
                            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                            byte[] cipherBytes = new byte[plainBytes.Length];
                            byte[] tag = new byte[TagSize];

                            aes.Encrypt(iv, plainBytes, cipherBytes, tag);

                            byte[] encryptedData = new byte[IvSize + cipherBytes.Length + TagSize];
                            Array.Copy(iv, 0, encryptedData, 0, IvSize);
                            Array.Copy(cipherBytes, 0, encryptedData, IvSize, cipherBytes.Length);
                            Array.Copy(tag, 0, encryptedData, IvSize + cipherBytes.Length, TagSize);

                            return Convert.ToBase64String(encryptedData);
                        }
                    }
                    else
                    {
                        return null!;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            public static string Decrypt(string? cipherText)
            {
                try
                {
                    if (cipherText != null)
                    {
                        var key = "GradeFlowWPF" + ComplexComputation();
                        byte[] keyBytes = GetKey(key);

                        cipherText = cipherText.PadRight(cipherText.Length + (4 - cipherText.Length % 4) % 4, '=');
                        byte[] cipherData = Convert.FromBase64String(cipherText);

                        byte[] iv = new byte[IvSize];
                        byte[] tag = new byte[TagSize];
                        byte[] cipherBytes = new byte[cipherData.Length - IvSize - TagSize];

                        Array.Copy(cipherData, 0, iv, 0, IvSize);
                        Array.Copy(cipherData, IvSize, cipherBytes, 0, cipherBytes.Length);
                        Array.Copy(cipherData, IvSize + cipherBytes.Length, tag, 0, TagSize);

                        using (AesGcm aes = new AesGcm(keyBytes, TagSize))
                        {
                            byte[] plainBytes = new byte[cipherBytes.Length];
                            aes.Decrypt(iv, cipherBytes, tag, plainBytes);
                            return Encoding.UTF8.GetString(plainBytes);
                        }
                    }
                    else
                    {
                        return null!;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            private static byte[] GetKey(string key)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                }
            }

            private static string ComplexComputation()
            {
                int[] values = { 1012, 3, 5, 7, 4 };
                int sum = 0;
                for (int i = 0; i < values.Length; i++)
                {
                    sum += values[i] * (i % 2 == 0 ? 2 : 3);
                }
                sum -= Fibonacci(5) * 10;
                sum += Factorial(3);
                return $"{sum}ects2025";
            }

            private static int Fibonacci(int n)
            {
                if (n <= 1) return n;
                return Fibonacci(n - 1) + Fibonacci(n - 2);
            }

            private static int Factorial(int n)
            {
                if (n <= 1) return 1;
                return n * Factorial(n - 1);
            }
        }

        private readonly GradeFlowContext _context;

        public ExamRepository()
        {
            _context = new GradeFlowContext();
        }

        public string Decrypt(string encryptedString)
        {
            try
            {
                if (!string.IsNullOrEmpty(encryptedString))
                {
                    return LOL.Decrypt(encryptedString);
                }
                else
                {
                    return encryptedString;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Decrypt] Ошибка: {ex.Message}");
                return "Error";
            }
        }

        private void Save()
        {
            _context.SaveChanges();
        }

        public void AddExam(Exam exam)
        {
            try
            {
                if (exam == null) throw new ArgumentNullException(nameof(exam));
                exam.ExamName = LOL.Encrypt(exam.ExamName);
                _context.Exams.Add(exam);
                Save();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AddExam] Оошибка: {ex.Message}");
            }
        }

        public List<ExamItemViewModel> GetAllExamsWithGroup()
        {
            try
            {
                List<Exam> exams = _context.Exams
                            .AsNoTracking()
                            .Include(e => e.Discipline)
                            .Include(e => e.GroupsExams)
                            .ThenInclude(ge => ge.Group)
                            .OrderBy(e => e.OpenDate)
                            .ThenBy(e => e.OpenTime)
                            .ToList();

                return exams.Select(e => new ExamItemViewModel(e)
                {
                    DisciplineName = Decrypt(e.Discipline.DisciplineName),
                    ExamName = Decrypt(e.ExamName)
                })
                .ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetExamWithGroup] Ошибка: {ex.Message}");
                return new List<ExamItemViewModel>();
            }
        }

        public Exam? GetExamById(Guid examId)
        {
            try
            {
                Exam? exam = _context.Exams.Include(e => e.Discipline)
                    .AsNoTracking()
                    .Where(e => e.ExamId == examId)
                    .FirstOrDefault();

                if (exam != null)
                {
                    exam.ExamName = Decrypt(exam.ExamName);
                    exam.Discipline.DisciplineName = Decrypt(exam.Discipline.DisciplineName);
                }

                return exam;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DeleteExam] Ошибка: {ex.Message}");
                return null;
            }
        }

        public void RemoveQuestion(int questionId)
        {
            try
            {
                var question = _context.Questions
                    .Include(q => q.QuestionAnswers)
                    .FirstOrDefault(q => q.QuestionId == questionId);

                if (question == null) return;
                _context.QuestionAnswers.RemoveRange(question.QuestionAnswers);
                _context.Questions.Remove(question);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RemoveQuestion] Ошибка: {ex.Message}");
            }
        }

        /*        public bool RemoveExam(Guid examId)
                {
                    try
                    {
                        Exam? exam = _context.Exams
                                     .Include(e => e.GroupsExams)
                                     .FirstOrDefault(e => e.ExamId == examId);

                        if (exam == null)
                            return false;

                        List<ExamTest> examTestList = _context.ExamTests
                                                .Where(e => e.ExamId == examId)
                                                .ToList();

                        HashSet<int> examTestIds = examTestList.Select(et => et.ExamTestId).ToHashSet();

                        List<TopicsExamTest> topicExamTestList = _context.TopicsExamTests
                            .Where(t => examTestIds.Contains(t.ExamTestId))
                            .ToList();

                        _context.GroupsExams.RemoveRange(exam.GroupsExams);
                        _context.TopicsExamTests.RemoveRange(topicExamTestList);
                        _context.ExamTests.RemoveRange(examTestList);
                        _context.Exams.Remove(exam);
                        Save();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[RemoveExam] Ошибка: {ex.Message}");
                        return false;
                    }
                }*/

        public bool RemoveExam(Guid examId)
        {
            try
            {
                Debug.WriteLine($"[RemoveExam] Попытка найти экзамен с ID: {examId}");

                // Загружаем экзамен с нужными связями
                Exam? exam = _context.Exams
                    .Include(e => e.GroupsExams)
                    .Include(e => e.StudentExamResults)
                    .Include(e => e.ExamTests)
                        .ThenInclude(et => et.Questions)
                            .ThenInclude(q => q.QuestionAnswers)
                    .Include(e => e.ExamTests)
                        .ThenInclude(et => et.TopicsExamTests)
                    .FirstOrDefault(e => e.ExamId == examId);

                if (exam == null)
                {
                    Debug.WriteLine("[RemoveExam] Экзамен не найден.");
                    return false;
                }

                Debug.WriteLine("[RemoveExam] Экзамен найден.");

                // Удаляем результаты студентов
                if (exam.StudentExamResults.Any())
                {
                    Debug.WriteLine($"[RemoveExam] Удаляется {exam.StudentExamResults.Count} результатов студентов.");
                    _context.StudentExamResults.RemoveRange(exam.StudentExamResults);
                }

                // Удаляем связи групп
                if (exam.GroupsExams.Any())
                {
                    Debug.WriteLine($"[RemoveExam] Удаляется {exam.GroupsExams.Count} связей GroupsExam.");
                    _context.GroupsExams.RemoveRange(exam.GroupsExams);
                }

                // Обход всех тестов экзамена
                foreach (var examTest in exam.ExamTests.ToList())
                {
                    // Удаляем TopicsExamTests
                    if (examTest.TopicsExamTests.Any())
                    {
                        Debug.WriteLine($"[RemoveExam] Удаляется {examTest.TopicsExamTests.Count} TopicsExamTests.");
                        _context.TopicsExamTests.RemoveRange(examTest.TopicsExamTests);
                    }

                    // Удаляем вопросы и их ответы
                    foreach (var question in examTest.Questions.ToList())
                    {
                        if (question.QuestionAnswers.Any())
                        {
                            Debug.WriteLine($"[RemoveExam] Удаляется {question.QuestionAnswers.Count} QuestionAnswers.");
                            _context.QuestionAnswers.RemoveRange(question.QuestionAnswers);
                        }

                        Debug.WriteLine("[RemoveExam] Удаляется Question.");
                        _context.Questions.Remove(question);
                    }

                    Debug.WriteLine("[RemoveExam] Удаляется ExamTest.");
                    _context.ExamTests.Remove(examTest);
                }

                // Удаляем сам экзамен
                _context.Exams.Remove(exam);

                // Сохраняем изменения
                Save();

                Debug.WriteLine("[RemoveExam] Удаление завершено успешно.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RemoveExam] Ошибка при удалении: {ex.Message}");
                return false;
            }
        }

        public bool UpdateExam(Exam exam)
        {
            try
            {
                Exam? existingExam = _context.Exams.Find(exam.ExamId);
                if (existingExam == null)
                {
                    return false;
                }
                existingExam.ExamName = LOL.Encrypt(exam.ExamName);
                existingExam.OpenDate = exam.OpenDate;
                existingExam.OpenTime = exam.OpenTime;
                existingExam.DisciplineId = exam.DisciplineId;
                existingExam.OwnerTeacherId = exam.OwnerTeacherId;

                Debug.WriteLine($"[UpdateExam] Обновлён ExamId: {exam.ExamId}");
                Save();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UpdateExam] Ошибка: {ex.Message}");
                return false;
            }
        }

        public bool HasAnyQuestion(Exam exam)
        {
            try
            {
                return _context.Questions.Any(q => q.ExamTest.Exam.ExamId == exam.ExamId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[HasAnyQuestion] Ошибка: {ex.Message}");
                return false;
            }
        }

        public ExamTest GetExamTestByExamId(Guid examId)
        {
            try
            {
                ExamTest? examTest = _context.ExamTests
                    .AsNoTracking()
                    .Include(et => et.Questions)
                        .ThenInclude(q => q.QuestionAnswers)
                    .FirstOrDefault(et => et.ExamId == examId);
                if (examTest == null)
                {
                    return new ExamTest();
                }
                return examTest;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetExamTestByExamId] Ошибка: {ex.Message}");
                return new ExamTest();
            }
        }

        public List<TopicsDiscipline> GetTopicsWithQuestionsByDisciplineId(int disciplineId)
        {
            try
            {
                return _context.TopicsDisciplines.AsNoTracking().Include(t => t.Questions).ThenInclude(q => q.QuestionAnswers).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetTopicsWithQuestionsByDisciplineId] Ошибка: {ex.Message}");
                return new List<TopicsDiscipline>();
            }
        }

        public bool AddQuestionToExam(Guid examId, Question question)
        {
            try
            {
                var examTest = _context.ExamTests.FirstOrDefault(et => et.ExamId == examId);

                if (examTest == null)
                {
                    examTest = new ExamTest
                    {
                        ExamId = examId,
                        TimeToComplete = 60,
                        TotalPoints = 0
                    };
                    _context.ExamTests.Add(examTest);
                    Save();
                }

                question.QuestionText = LOL.Encrypt(question.QuestionText);
                question.ExamTestId = examTest.ExamTestId;

                question.QuestionAnswers = question.QuestionAnswers.Select(answer =>
                    {
                        if (!string.IsNullOrWhiteSpace(answer.QuestionAnswerText))
                            answer.QuestionAnswerText = LOL.Encrypt(answer.QuestionAnswerText);
                        return answer;
                    })
                    .ToList();
                _context.Questions.Add(question);
                Save();

                examTest.TotalPoints += Convert.ToByte(question.QuestionAnswers.Count(answer => answer.IsCorrect));

                _context.ExamTests.Update(examTest);
                Save();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AddQuestionToExam] Ошибка: {ex.Message}");

                if (ex is DbUpdateException dbEx && dbEx.InnerException != null)
                {
                    Debug.WriteLine($"[AddQuestionToExam] Inner: {dbEx.InnerException.Message}");
                }

                return false;
            }
        }

        public int RecalculateTotalPoints(int examTestId)
        {
            var examTest = _context.ExamTests
                .Include(et => et.Questions)
                .ThenInclude(q => q.QuestionAnswers)
                .FirstOrDefault(et => et.ExamTestId == examTestId);

            if (examTest == null)
                return 55;

            int totalPoints = 0;

            foreach (var question in examTest.Questions)
            {
                if (question.IsSelected) totalPoints += question.QuestionAnswers.Count(a => a.IsCorrect);
            }

            examTest.TotalPoints = Convert.ToByte(totalPoints);
            _context.ExamTests.Update(examTest);
            _context.SaveChanges();

            return totalPoints;
        }

        public List<QuestionType> GetQuestionTypes()
        {
            try
            {
                List<QuestionType> types = _context.QuestionTypes.AsNoTracking().ToList();

                return types.Select(t =>
                {
                    t.QuestionTypeName = Decrypt(t.QuestionTypeName);
                    return t;
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetTopicsWithQuestionsByDisciplineId] Ошибка: {ex.Message}");
                return new List<QuestionType>();
            }
        }

        public ExamTest CreateExamTest(Guid examId)
        {
            try
            {
                var test = new ExamTest { ExamId = examId, TimeToComplete = 60, TotalPoints = 0 };
                _context.ExamTests.Add(test);
                _context.SaveChanges();
                return test;
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine($"[CreateExamTest] DbUpdateException: {ex.Message}");
                if (ex.InnerException != null)
                    Debug.WriteLine($"[CreateExamTest] Inner: {ex.InnerException.Message}");
                return new ExamTest();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CreateExamTest] Ошибка: {ex.Message}");
                return new ExamTest();
            }
        }

        public void UpdateExamTest(ExamTest updatedExamTest)
        {
            try
            {
                if (updatedExamTest == null)
                    throw new ArgumentNullException(nameof(updatedExamTest));

                var existingExamTest = _context.ExamTests
                    .FirstOrDefault(et => et.ExamTestId == updatedExamTest.ExamTestId);

                if (existingExamTest != null)
                {
                    existingExamTest.TimeToComplete = updatedExamTest.TimeToComplete;
                    existingExamTest.TotalPoints = updatedExamTest.TotalPoints;

                    _context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UpdateExamTest] Ошибка: {ex.Message}");
            }
        }

        public List<TopicsExamTest> GetSelectedTopicsByTestId(int examTestId)
        {
            return _context.TopicsExamTests
                .Where(tet => tet.ExamTestId == examTestId)
                .ToList();
        }

        public void SaveSelectedTopics(int examTestId, List<TopicWithQuestionsViewModel> selectedTopics)
        {
            var existing = _context.TopicsExamTests.Where(tet => tet.ExamTestId == examTestId);
            _context.TopicsExamTests.RemoveRange(existing);

            var newItems = selectedTopics
                .Where(t => t.IsSelected)
                .Select(t => new TopicsExamTest
                {
                    ExamTestId = examTestId,
                    TopicId = t.TopicId,
                    IsSelected = true
                });

            var topics = _context.TopicsDisciplines.ToList();

            _context.TopicsExamTests.AddRange(newItems);
            _context.TopicsDisciplines.UpdateRange(topics);
            _context.SaveChanges();
        }

        public void SaveSelectedQuestions(int examTestId, List<Question> selectedQuestions)
        {
            var existing = _context.Questions.ToList();

            foreach (var question in existing)
            {
                var temp = selectedQuestions.FirstOrDefault(q => q.QuestionId == question.QuestionId);

                if (temp != null) question.IsSelected = temp.IsSelected;
            }

            _context.Questions.UpdateRange(existing);
            _context.SaveChanges();
        }

        public List<ExamItemViewModel> GetUpcomingExamsForStudent(int? studentId)
        {
            if (studentId != null)
            {
                var student = _context.Students.Where(s => s.StudentId == studentId).FirstOrDefault();
                var now = DateTime.Now;
                var today = DateOnly.FromDateTime(now);
                var currentTime = TimeOnly.FromDateTime(now);
                var plus10 = currentTime.AddMinutes(10);

                var exams = _context.Exams
                    .Where(e => e.OpenDate == today && e.GroupsExams.Any(ge => ge.GroupId == student.GroupId))
                    .Include(e => e.Discipline)
                    .Include(e => e.GroupsExams)
                    .ThenInclude(ge => ge.Group)
                    .AsNoTracking()
                    .ToList();
 
                return exams.Select(e => new ExamItemViewModel(e)
                {
                    DisciplineName = Decrypt(e.Discipline.DisciplineName),
                    ExamName = Decrypt(e.ExamName)
                })
                .ToList();
            }
            else
            {
                return new List<ExamItemViewModel>();
            }
        }
    }
}