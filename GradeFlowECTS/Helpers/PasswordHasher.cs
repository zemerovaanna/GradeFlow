using System.Security.Cryptography;

namespace GradeFlowECTS.Helpers
{
    /// <summary>
    /// Статический класс для хеширования и проверки паролей с использованием PBKDF2 с SHA-256.
    /// </summary>
    /// <remarks>
    /// Реализует безопасное хеширование паролей с использованием криптографической соли
    /// и множества итераций для защиты от атак перебором.
    /// </remarks>
    internal static class PasswordHasher
    {
        private const int HashSize = 32;
        private const int Iterations = 100_000;
        private const int SaltSize = 16;

        /// <summary>
        /// Хеширует пароль с использованием PBKDF2 с SHA-256.
        /// </summary>
        /// <param name="password">Пароль для хеширования.</param>
        /// <returns>Строка в Base64, содержащая соль и хеш пароля.</returns>
        /// <remarks>
        /// Генерирует случайную соль, применяет 100,000 итераций PBKDF2 с SHA-256,
        /// и возвращает соль и хеш в виде объединенного массива байтов, закодированного в Base64.
        /// </remarks>
        internal static string HashPassword(string password)
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);

                using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                {
                    byte[] hash = pbkdf2.GetBytes(HashSize);
                    byte[] hashBytes = new byte[SaltSize + HashSize];
                    Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                    Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                    return Convert.ToBase64String(hashBytes);
                }
            }
        }

        /// <summary>
        /// Проверяет пароль на соответствие сохраненному хешу.
        /// </summary>
        /// <param name="password">Пароль для проверки.</param>
        /// <param name="storedHash">Сохраненный хеш в формате Base64.</param>
        /// <returns>True, если пароль соответствует хешу, иначе False.</returns>
        /// <remarks>
        /// Извлекает соль из сохраненного хеша, применяет те же параметры хеширования
        /// и сравнивает результат с сохраненным хешем.
        /// </remarks>
        internal static bool VerifyPassword(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                for (int i = 0; i < HashSize; i++)
                {
                    if (hashBytes[SaltSize + i] != hash[i])
                        return false;
                }
            }

            return true;
        }
    }
}