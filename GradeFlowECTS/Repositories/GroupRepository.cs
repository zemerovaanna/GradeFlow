using System.Diagnostics;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.ViewModel.Items;
using Microsoft.EntityFrameworkCore;

namespace GradeFlowECTS.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly GradeFlowContext _context;

        public GroupRepository(GradeFlowContext context) => _context = context ?? throw new ArgumentNullException(nameof(context));

        public List<GroupItemViewModel> GetAllGroups()
        {
            try
            {
                List<GroupItemViewModel> groups = _context.Groups
                    .AsNoTracking()
                    .ToList()
                    .Select(g => new GroupItemViewModel(g))
                    .ToList();

                if (groups == null || !groups.Any())
                {
                    return new List<GroupItemViewModel>();
                }

                return groups;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetAllGroups] Ошибка: {ex.Message}");
                return new List<GroupItemViewModel>();
            }
        }
    }
}