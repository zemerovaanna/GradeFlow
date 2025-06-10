using GradeFlowECTS.Models;

namespace GradeFlowECTS.Interfaces
{
    public interface IDisciplineRepository
    {
        /// <summary>
        /// Возвращает список всех дисциплин.
        /// </summary>
        /// <returns>Список объектов типа Discipline.</returns>
        List<Discipline> GetAllDisciplines();

        /// <summary>
        /// Возвращает список дисциплин.
        /// </summary>
        /// <param name="count">Количесвто первых дисциплин из списка.</param>
        /// <returns>Список объектов типа Discipline.</returns>
        List<Discipline> GetTopDisciplines(int count);

        /// <summary>
        /// Возвращает дисциплину по указанному идентификатору.
        /// </summary>
        /// <param name="discipline">Идентификатор дисциплины.</param>
        /// <returns>Объект типа дисциплина.</returns>
        Discipline GetDisciplineById(int discipline);
    }
}