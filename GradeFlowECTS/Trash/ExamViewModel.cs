using System.Collections.ObjectModel;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Models;
using Microsoft.EntityFrameworkCore;

namespace GradeFlowECTS.Trash
{
    public class ExamViewModel
    {
        private readonly GradeFlowContext _context = new();

        public ObservableCollection<GroupViewModel> Groups { get; set; } = new();

        public ExamViewModel(Guid examId)
        {
            var groupsWithExam = _context.GroupsExams
    .Include(ge => ge.Group)
        .ThenInclude(g => g.Students)
            .ThenInclude(s => s.User)
    .Include(ge => ge.Group)
        .ThenInclude(g => g.Students)
            .ThenInclude(s => s.StudentExamResults)
    .Where(ge => ge.ExamId == examId)
    .ToList();


            foreach (var groupExam in groupsWithExam)
            {
                var groupVM = new GroupViewModel(groupExam.Group.GroupNumber);

                foreach (var student in groupExam.Group.Students)
                {
                    var result = student.StudentExamResults
                        .FirstOrDefault(r => r.ExamId == examId);

                    if (result == null)
                    {
                        result = new StudentExamResult
                        {
                            StudentId = student.StudentId,
                            ExamId = examId,
                            VariantNumber = new Random().Next(1, 31)
                        };
                        _context.StudentExamResults.Add(result);
                        _context.SaveChanges();
                    }

                    groupVM.Students.Add(new StudentViewModel(result, _context));
                }

                Groups.Add(groupVM);
            }
        }
    }
}