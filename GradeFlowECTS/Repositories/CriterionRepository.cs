using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Models;
using Microsoft.EntityFrameworkCore;

namespace GradeFlowECTS.Repositories
{
    public class CriterionRepository
    {
        private readonly GradeFlowContext _context;

        public CriterionRepository()
        {
            _context = new GradeFlowContext();
        }

        public List<Criterion> GetAllCriteriaWithDetails()
        {
            return _context.Criteria
                .Include(c => c.Module)
                .OrderBy(c => c.CriterionNumber)
                .Include(c => c.ScoreOptions)
                .ToList();
        }

        public List<Module> GetAllModules()
        {
            return _context.Modules.ToList();
        }

        public void DeleteCriterion(Criterion criterion)
        {
            _context.ScoreOptions.RemoveRange(criterion.ScoreOptions);
            _context.Criteria.Remove(criterion);
            _context.SaveChanges();
        }

        public void SaveChanges(List<Criterion> updatedCriteria)
        {
            foreach (var updated in updatedCriteria)
            {
                var existing = _context.Criteria
                    .Include(c => c.ScoreOptions)
                    .FirstOrDefault(c => c.CriterionId == updated.CriterionId);

                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(updated);

                    foreach (var opt in updated.ScoreOptions)
                    {
                        var exOpt = existing.ScoreOptions.FirstOrDefault(o => o.ScoreOptionIdId == opt.ScoreOptionIdId);
                        if (exOpt != null)
                            _context.Entry(exOpt).CurrentValues.SetValues(opt);
                        else
                            existing.ScoreOptions.Add(opt);
                    }
                }
            }

            _context.SaveChanges();
        }

        public void AddNewCriterion(Criterion criterion)
        {
            if (_context.Modules.Any(m => m.ModuleId == criterion.ModuleId))
            {
                _context.Entry(criterion.Module).State = EntityState.Unchanged;
            }

            _context.Criteria.Add(criterion);
            _context.SaveChanges();
        }
    }
}