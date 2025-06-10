using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.ViewModel.Items;
using Microsoft.EntityFrameworkCore;

namespace GradeFlowECTS.Repositories
{
    public class GroupsExamRepository : IGroupsExamRepository
    {
        private readonly GradeFlowContext _context;

        public GroupsExamRepository(GradeFlowContext context) => _context = context ?? throw new ArgumentNullException(nameof(context));
        
        private void Save()
        {
            _context.SaveChanges();
        }

        public void AddGroupsExam(List<GroupItemViewModel> groups, Guid examGuid)
        {
            foreach (GroupItemViewModel group in groups)
            {
                GroupsExam groupExam = new GroupsExam
                {
                    ExamId = examGuid,
                    GroupId = group.GroupId
                };

                _context.GroupsExams.Add(groupExam);
            }
            Save();
        }

        public void UpdateGroupsExam(List<GroupItemViewModel> newGroups, Guid examGuid)
        {
            var existingGroups = _context.GroupsExams.Where(ge => ge.ExamId == examGuid);
            _context.GroupsExams.RemoveRange(existingGroups);
            Save();
            AddGroupsExam(newGroups, examGuid);
        }

        public List<GroupItemViewModel> GetGroupsExamById(Guid examGuid)
        {
            return _context.GroupsExams.Where(ge => ge.ExamId == examGuid).Include(ge => ge.Group).AsNoTracking().ToList().Select(ge => new GroupItemViewModel(ge.Group)).ToList();
        }
    }
}