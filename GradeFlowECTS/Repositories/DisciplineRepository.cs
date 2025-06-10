using System.Diagnostics;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using Microsoft.EntityFrameworkCore;

namespace GradeFlowECTS.Repositories
{
    public class DisciplineRepository : IDisciplineRepository
    {
        private readonly GradeFlowContext _context;
        private readonly ICryptographyService _cryptoryptographyService;
        private readonly IConfigurationService _configurationService;

        public DisciplineRepository(GradeFlowContext context, ICryptographyService cryptoryptographyService, IConfigurationService configurationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cryptoryptographyService = cryptoryptographyService ?? throw new ArgumentNullException(nameof(cryptoryptographyService));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        }

        public List<Discipline> GetAllDisciplines()
        {
            try
            {
                List<Discipline> disciplines = _context.Disciplines.OrderBy(d => d.DisciplineId).AsNoTracking().ToList();

                return disciplines.Select(d =>
                {
                    try
                    {

                        d.DisciplineName = _cryptoryptographyService.Decrypt(d.DisciplineName, _configurationService.GetEncryptionKey());
                        return d;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[GetTopDisciplines] Ошибка при дешифровании дисциплины {d.DisciplineId}: {ex.Message}");
                        return d;
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetTopDisciplines] Ошибка: {ex.Message}");
                return new List<Discipline>();
            }
        }

        public Discipline GetDisciplineById(int discipline)
        {
            return _context.Disciplines.Where(d => d.DisciplineId == discipline).AsNoTracking().FirstOrDefault();
        }

        public List<Discipline> GetTopDisciplines(int count)
        {
            try
            {
                List<Discipline> disciplines = _context.Disciplines.OrderBy(d => d.DisciplineId).Take(count).AsNoTracking().ToList();

                return disciplines.Select(d =>
                {
                    try
                    {
                        d.DisciplineName = _cryptoryptographyService.Decrypt(d.DisciplineName, _configurationService.GetEncryptionKey());
                        return d;
                    }
                    catch
                    {
                        return d;
                    }

                }).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetTopDisciplines] Ошибка: {ex.Message}");
                return new List<Discipline>();
            }
        }
    }
}