using GradeFlowECTS.Models;

namespace GradeFlowECTS.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с ролями пользователей.
    /// </summary>
    public interface IUserRoleRepository
    {
        /// <summary>
        /// Получает список всех ролей пользователей в расшифрованном виде.
        /// </summary>
        /// <returns>Список ролей пользователей.</returns>
        List<UserRole> GetAllRoles();

        /// <summary>
        /// Получает название роли по её идентификатору в расшифрованном виде.
        /// </summary>
        /// <param name="roleId">Идентификатор роли.</param>
        /// <returns>Название роли.</returns>
        string GetRoleNameById(int roleId);
    }
}