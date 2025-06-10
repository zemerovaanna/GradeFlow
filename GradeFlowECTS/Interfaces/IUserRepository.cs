using GradeFlowECTS.Models;
using GradeFlowECTS.ViewModel.Items;

namespace GradeFlowECTS.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с пользователями системы.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Добавляет нового студента асинхронно.
        /// </summary>
        /// <param name="student">Данные студента.</param>
        void AddStudent(Student student);

        /// <summary>
        /// Добавляет список новых студентов асинхронно.
        /// </summary>
        /// <param name="newStudents">Коллекция студентов.</param>
        void AddStudents(List<User> newStudents);

        /// <summary>
        /// Добавляет нового преподавателя асинхронно.
        /// </summary>
        /// <param name="teacher">Данные преподавателя.</param>
        void AddTeacher(Teacher teacher);

        /// <summary>
        /// Назначает студента в учебную группу асинхронно.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="groupId">Идентификатор группы.</param>
        void AssignStudentToGroup(Guid userId, int groupId);

        /// <summary>
        /// Назначает преподавателя с использованием кода асинхронно.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="teacherCode">Код преподавателя.</param>
        void AssignTeacherWithCode(Guid userId, string teacherCode);

        /// <summary>
        /// Создает нового пользователя асинхронно.
        /// </summary>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="middleName">Отчество (опционально).</param>
        /// <param name="mail">Электронная почта.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="roleId">Идентификатор роли пользователя.</param>
        /// <param name="status">Статус активности.</param>
        /// <returns>Созданный пользователь.</returns>
        User CreateNewUser(string lastName, string firstName, string? middleName, string mail, string password, int roleId, bool status);

        /// <summary>
        /// Сохраняет пользователя в репозитории асинхронно.
        /// </summary>
        /// <param name="user">Данные пользователя.</param>
        void CreateUser(User user);

        /// <summary>
        /// Проверяет, существует ли пользователь с указанной почтой.
        /// </summary>
        /// <param name="mail">Электронная почта.</param>
        /// <returns>True, если пользователь существует, иначе False.</returns>
        bool DoesEmailExist(string mail);

        /// <summary>
        /// Проверяет, существует ли пользователь с указанными ФИО.
        /// </summary>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="middleName">Отчество (опционально).</param>
        /// <returns>True, если пользователь существует, иначе False.</returns>
        bool DoesFullNameExist(string lastName, string firstName, string? middleName);

        /// <summary>
        /// Проверяет, существует ли пользователь с указанным идентификатором асинхронно.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>True, если пользователь существует, иначе False.</returns>
        bool DoesUserIdExist(Guid id);

        /// <summary>
        /// Получает пользователя по электронной почте.
        /// </summary>
        /// <param name="mail">Электронная почта.</param>
        /// <returns>Найденный пользователь или null.</returns>
        User? GetUserByEmail(string mail);

        /// <summary>
        /// Обновляет пароль пользователя.
        /// </summary>
        /// <param name="mail">Электронная почта.</param>
        /// <param name="newPassword">Новый пароль.</param>
        /// <returns>Результат операции.</returns>
        string UpdatePassword(string mail, string newPassword);

        /// <summary>
        /// Дешифрует электронную почту пользователя.
        /// </summary>
        /// <param name="encryptedEmail">Зашифрованная почта.</param>
        /// <returns>Расшифрованная почта.</returns>
        string DecryptEmail(string encryptedEmail);

        /// <summary>
        /// Возвращает идентификатор преподавателя по уникальному идентификатору пользователя.
        /// </summary>
        /// <param name="userGuid">Уникальный идентификатор пользователя.</param>
        /// <returns>Целое число - идентификатор преподавателя.</returns>
        int GetTeacherIdByuserGuid(Guid userGuid);
        int GetStudentIdByuserGuid(Guid userGuid);

        /// <summary>
        /// Возвращает список всех пользователей.
        /// </summary>
        /// <returns>Список объектов типа User.</returns>
        List<UserItemViewModel> GetAllUserItems();

        /// <summary>
        /// Возвращает список всех студентов по указанному идентификатору группы.
        /// </summary>
        /// <param name="groupId">Идентификатор группы.</param>
        /// <returns>Список объектов типа StudentItemViewModel.</returns>
        List<StudentItemViewModel> GetStudentsByGroupId(int groupId);

        List<string> GetDecrypted(List<string> strings);

        Task<bool> DeleteUserAsync(Guid userId);
    }
}