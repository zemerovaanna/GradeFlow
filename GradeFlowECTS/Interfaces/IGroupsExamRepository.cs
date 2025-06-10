using GradeFlowECTS.ViewModel.Items;

namespace GradeFlowECTS.Interfaces
{
    public interface IGroupsExamRepository
    {
        /// <summary>
        /// Добавляет указанные группы к экзамену с заданным идентификатором.
        /// </summary>
        /// <param name="groups">Список групп для добавления к экзамену.</param>
        /// <param name="examGuid">Уникальный идентификатор экзамена, к которому добавляются группы.</param>
        void AddGroupsExam(List<GroupItemViewModel> groups, Guid examGuid);

        /// <summary>
        /// Обновление данных о группах экзамена.
        /// </summary>
        /// <param name="newGroups">Список объектов типа GroupItemViewModel.</param>
        /// <param name="examGuid">Уникальный идентификатор экзамена.</param>
        void UpdateGroupsExam(List<GroupItemViewModel> newGroups, Guid examGuid);

        /// <summary>
        /// Возвращает группы экзамена по указанному уникальному идентификатору.
        /// </summary>
        /// <param name="examGuid">Уникальный идентификатор экзамена.</param>
        /// <returns>Список объектов типа GroupItemViewModel.</returns>
        List<GroupItemViewModel> GetGroupsExamById(Guid examGuid);
    }
}