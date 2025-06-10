using GradeFlowECTS.Models;
using GradeFlowECTS.ViewModel.Items;

namespace GradeFlowECTS.Interfaces
{
    public interface IGroupRepository
    {
        /// <summary>
        /// Возвращает список всех групп.
        /// </summary>
        /// <returns>Список объектов типа Group.</returns>
        List<GroupItemViewModel> GetAllGroups();
    }
}