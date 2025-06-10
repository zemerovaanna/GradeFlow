using System.Diagnostics;
using System.Windows;
using GradeFlowECTS.Helpers;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using GradeFlowECTS.ViewModel.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GradeFlowECTS.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly GradeFlowContext _context;
        private readonly IConfigurationService _configurationService;
        private readonly ICryptographyService _cryptographyService;

        public UserRepository(GradeFlowContext context, IConfigurationService configurationService, ICryptographyService cryptographyService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
            _cryptographyService = cryptographyService ?? throw new ArgumentNullException(nameof(cryptographyService));
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Students)
                .Include(u => u.Teachers)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return false;

            if (user.Students.Any())
            {
                _context.Students.RemoveRange(user.Students);
            }

            if (user.Teachers.Any())
            {
                _context.Teachers.RemoveRange(user.Teachers);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        private string GetKey()
        {
            try
            {
                return _configurationService.GetEncryptionKey();
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        private string Decrypt(string key, string encryptedString)
        {
            try
            {
                if (!string.IsNullOrEmpty(encryptedString))
                {
                    return _cryptographyService.Decrypt(encryptedString, key);
                }
                else
                {
                    return encryptedString;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Decrypt] Ошибка: {ex.Message}");
                return "Error";
            }
        }

        private void Save()
        {
            _context.SaveChanges();
        }

        public void AddStudent(Student student)
        {
            try
            {
                if (student != null)
                {
                    _context.Students.Add(student);
                    Save();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AddStudent] Ошибка: {ex.Message}");
            }
        }

        public void AddStudents(List<User> newStudents)
        {
            try
            {
                if (newStudents != null)
                {
                    string key = GetKey();
                    foreach (User student in newStudents)
                    {
                        student.FirstName = _cryptographyService.Encrypt(student.FirstName, key);
                        student.LastName = _cryptographyService.Encrypt(student.LastName, key);
                        student.MiddleName = _cryptographyService.Encrypt(student.MiddleName, key);
                        student.Mail = _cryptographyService.Encrypt(student.Mail, key);
                    }

                    _context.AddRange(newStudents);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AddStudents] Ошибка: {ex.Message}");
            }
        }

        public void AddTeacher(Teacher teacher)
        {
            try
            {
                _context.Teachers.Add(teacher);
                Save();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AddTeacher] Ошибка: {ex.Message}");
            }
        }

        public void AssignStudentToGroup(Guid userId, int groupId)
        {
            try
            {
                Student student = new Student { UserId = userId, GroupId = groupId };
                AddStudent(student);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AssignStudentToGroup] Ошибка: {ex.Message}");
            }
        }

        public void AssignTeacherWithCode(Guid userId, string teacherCode)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(teacherCode))
                {
                    Teacher teacher = new Teacher { UserId = userId, Code = teacherCode };
                    AddTeacher(teacher);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AssignTeacherWithCode] Ошибка: {ex.Message}");
            }
        }

        public User CreateNewUser(string lastName, string firstName, string? middleName, string mail, string password, int roleId, bool status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lastName))
                    throw new ArgumentException(nameof(lastName));
                if (string.IsNullOrWhiteSpace(firstName))
                    throw new ArgumentException(nameof(firstName));
                if (string.IsNullOrWhiteSpace(mail))
                    throw new ArgumentException(nameof(mail));
                /*if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException(nameof(password));*/

                string key = GetKey();

                User newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    LastName = _cryptographyService.Encrypt(lastName, key),
                    FirstName = _cryptographyService.Encrypt(firstName, key),
                    MiddleName = middleName != null ? _cryptographyService.Encrypt(middleName, key) : null,
                    Mail = _cryptographyService.Encrypt(mail, key),
                    Password = !string.IsNullOrWhiteSpace(password) ? PasswordHasher.HashPassword(password) : null,
                    RoleId = roleId,
                    Status = status
                };

                CreateUser(newUser);

                return newUser;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CreateNewUser] Ошибка: {ex.Message}");
                return null;
            }
        }

        public void CreateUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                Save();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CreateUser] Ошибка: {ex.Message}");
            }
        }

        public bool DoesEmailExist(string mail)
        {
            try
            {
                string key = GetKey();

                bool exists = _context.Users
                    .AsNoTracking()
                    .AsEnumerable()
                    .Any(u => Decrypt(key, u.Mail!) == mail);

                return exists;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DoesEmailExist] Ошибка: {ex.Message}");
                MessageBox.Show("Пожалуйста, перезайдите в приложение.");
                return false;
            }
        }

        public bool DoesFullNameExist(string lastName, string firstName, string? middleName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lastName))
                    throw new ArgumentException(nameof(lastName));
                if (string.IsNullOrWhiteSpace(firstName))
                    throw new ArgumentException(nameof(firstName));

                string key = GetKey();

                return _context.Users
                       .Any(u => Decrypt(u.LastName!, key) == lastName &&
                       Decrypt(u.FirstName!, key) == firstName &&
                       (middleName == null ? u.MiddleName == null : Decrypt(u.MiddleName!, key) == middleName));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DoesFullNameExist] Ошибка: {ex.Message}");
                return false;
            }
        }

        public bool DoesUserIdExist(Guid id)
        {
            try
            {
                return _context.Users.Any(u => u.UserId == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DoesUserIdExist] Ошибка: {ex.Message}");
                return false;
            }
        }

        public List<User> GetAllUsersWithRoles()
        {
            try
            {
                return _context.Users
                    .Include(u => u.Role)
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetAllUsersWithRoles] Ошибка: {ex.Message}");
                return new List<User>();
            }
        }

        public User? GetUserByEmail(string mail)
        {
            try
            {
                string key = GetKey();

                return _context.Users
                    .AsNoTracking()
                    .AsEnumerable()
                    .FirstOrDefault(u => Decrypt(key, u.Mail!) == mail);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetUserByEmail] Ошибка: {ex.Message}");
                return null;
            }
        }

        public string UpdatePassword(string mail, string newPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mail))
                    throw new ArgumentException(nameof(mail));
                if (string.IsNullOrWhiteSpace(newPassword))
                    throw new ArgumentException(nameof(newPassword));

                var user = GetUserByEmail(mail);
                if (user == null)
                {
                    Debug.WriteLine($"[UpdatePassword] Пользователь с почтой {mail} не найден");
                    return "Пользователь не найден";
                }

                if (!user.Password.IsNullOrEmpty() && PasswordHasher.VerifyPassword(newPassword, user.Password!))
                {
                    Debug.WriteLine($"[UpdatePassword] Пользователь {mail} пытается установить тот же пароль");
                    return "Совпадение со старым";
                }

                user.Password = PasswordHasher.HashPassword(newPassword);
                _context.SaveChanges();
                return "Успех";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UpdatePassword] Ошибка: {ex.Message}");
                return $"Ошибка: {ex.Message}";
            }
        }

        public string DecryptEmail(string encryptedEmail)
        {
            try
            {
                string key = GetKey();
                return _cryptographyService.Decrypt(encryptedEmail, key);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DecryptEmail] Ошибка: {ex.Message}");
                return "Error";
            }
        }

        public int GetTeacherIdByuserGuid(Guid userGuid)
        {
            try
            {
                return _context.Teachers.Where(t => t.UserId == userGuid).Select(t => t.TeacherId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetTeacherIdByuserGuid] Ошибка: {ex.Message}");
                return 0;
            }
        }

        public List<UserItemViewModel> GetAllUserItems()
        {
            try
            {
                string key = _configurationService.GetEncryptionKey();
                return _context.Users
                    .Include(u => u.Role)
                    .AsNoTracking()
                    .ToList()
                    .Select(u =>
                    {
                        UserRole decryptedRole = new UserRole
                        {
                            RoleId = u.Role.RoleId,
                            RoleName = _cryptographyService.Decrypt(u.Role.RoleName, key),
                            Users = u.Role.Users
                        };

                        return new UserItemViewModel(u)
                        {
                            UserId = u.UserId,
                            LastName = _cryptographyService.Decrypt(u.LastName, key),
                            FirstName = _cryptographyService.Decrypt(u.FirstName, key),
                            MiddleName = _cryptographyService.Decrypt(u.MiddleName, key),
                            Mail = _cryptographyService.Decrypt(u.Mail, key),
                            Role = decryptedRole,
                            Status = u.Status
                        };
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetAllUserItems] Ошибка: {ex.Message}");
                return new List<UserItemViewModel>();
            }
        }

        public List<StudentItemViewModel> GetStudentsByGroupId(int groupId)
        {
            try
            {
                string key = _configurationService.GetEncryptionKey();

                List<Student> students = _context.Students
                    .Include(s => s.User)
                    .Include(s => s.Group)
                    .Where(s => s.GroupId == groupId)
                    .AsNoTracking()
                    .ToList();

                List<StudentItemViewModel> studentVMs = students.Select(s =>
                {
                    UserItemViewModel decryptedUser = new UserItemViewModel
                    {
                        UserId = s.User.UserId,
                        LastName = _cryptographyService.Decrypt(s.User.LastName, key),
                        FirstName = _cryptographyService.Decrypt(s.User.FirstName, key),
                        MiddleName = _cryptographyService.Decrypt(s.User.MiddleName, key),
                        Mail = _cryptographyService.Decrypt(s.User.Mail, key),
                        Role = s.User.Role,
                        Status = s.User.Status
                    };

                    StudentItemViewModel studentVM = new StudentItemViewModel(s)
                    {
                        UserItem = decryptedUser
                    };

                    return studentVM;

                }).ToList();

                return studentVMs;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetStudentsByGroupId] Ошибка: {ex.Message}");
                return new List<StudentItemViewModel>();
            }
        }

        public List<string> GetDecrypted(List<string> strings)
        {
            string key = GetKey();
            return strings.Select(s =>
            {
                return s = Decrypt(key, s);
            }).ToList();
        }

        public int GetStudentIdByuserGuid(Guid userGuid)
        {
            try
            {
                return _context.Students.Where(t => t.UserId == userGuid).Select(t => t.StudentId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetStudentIdByuserGuid] Ошибка: {ex.Message}");
                return 0;
            }
        }
    }
}