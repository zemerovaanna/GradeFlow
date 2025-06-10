using System.Diagnostics;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Interfaces;
using GradeFlowECTS.Models;
using Microsoft.EntityFrameworkCore;

namespace GradeFlowECTS.Repositories
{
    internal class UserRoleRepository : IUserRoleRepository
    {
        private readonly GradeFlowContext _context;
        private readonly IConfigurationService _configurationService;
        private readonly ICryptographyService _cryptographyService;

        public UserRoleRepository(GradeFlowContext context, IConfigurationService configurationService, ICryptographyService cryptographyService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
            _cryptographyService = cryptographyService ?? throw new ArgumentNullException(nameof(cryptographyService));
        }

        private string GetKey()
        {
            try
            {
                return _configurationService.GetEncryptionKey();
            }
            catch(Exception ex)
            {
                return "Error";
            }
        }

        public List<UserRole> GetAllRoles()
        {
            try
            {
                List<UserRole> roles = _context.UserRoles.AsNoTracking().ToList();

                string key = GetKey();

                return roles.Select(role =>
                {
                    try
                    {
                        role.RoleName = _cryptographyService.Decrypt(role.RoleName, key);
                        return role;
                    }
                    catch
                    {
                        role.RoleName = "Неопределено";
                        return role;
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetAllRoles] Ошибка: {ex.Message}");
                return new List<UserRole>();
            }
        }

        public string GetRoleNameById(int roleId)
        {
            try
            {
                UserRole role = _context.UserRoles.FirstOrDefault(r => r.RoleId == roleId);

                string key = GetKey();
                return string.IsNullOrEmpty(role.RoleName) ? "Неопределено" : _cryptographyService.Decrypt(role.RoleName, key);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetDecryptedRoleNameById] Ошибка: {ex.Message}");
                return "Неопределено";
            }
        }
    }
}