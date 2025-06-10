namespace GradeFlowECTS.Helpers
{
    /// <summary>
    /// Вспомогательный класс для работы с безопасностью.
    /// 
    /// Предоставляет функциональность для:
    /// - Генерации и верификации кодов подтверждения.
    /// - Генерации сложных паролей.
    /// 
    /// Особенности:
    /// - Генерирует коды с использованием русских букв и цифр.
    /// - Создает пароли, соответствующие стандартным требованиям сложности.
    /// </summary>
    internal static class SecurityHelper
    {
        private static readonly Dictionary<string, string> _verificationCodes = new Dictionary<string, string>();

        /// <summary>
        /// Генерирует 6-символьный верификационный код (чередует русские буквы и цифры)
        /// </summary>
        /// <returns>Сгенерированный код в формате "А1Б2В3"</returns>
        public static string GenerateCode()
        {
            Random random = new Random();
            string letters = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            string digits = "0123456789";

            char[] code = new char[6];
            for (int i = 0; i < 6; i++)
            {
                if (i % 2 == 0)
                {
                    code[i] = letters[random.Next(letters.Length)];
                }
                else
                {
                    code[i] = digits[random.Next(digits.Length)];
                }
            }

            return new string(code);
        }

        /// <summary>
        /// Сохраняет верификационный код для указанного email
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <param name="code">Сгенерированный верификационный код</param>
        public static void StoreCode(string email, string code)
        {
            _verificationCodes[email] = code;
        }

        /// <summary>
        /// Проверяет соответствие введенного кода сохраненному для email
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <param name="enteredCode">Введенный пользователем код</param>
        /// <returns>True если код верный, иначе False</returns>
        public static bool VerifyCode(string email, string enteredCode)
        {
            return _verificationCodes.TryGetValue(email, out var storedCode) && storedCode == enteredCode;
        }

        /// <summary>
        /// Генерирует безопасный пароль длиной 15 символов
        /// </summary>
        /// <returns>Пароль, содержащий буквы верхнего/нижнего регистра, цифры и спецсимволы</returns>
        public static string GeneratePassword()
        {
            Random random = new Random();
            byte length = 15;

            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialCharacters = "!@#$%^&*()_-+=<>?";

            string allCharacters = upperCase + lowerCase + digits + specialCharacters;
            char[] password = new char[length];

            password[0] = upperCase[random.Next(upperCase.Length)];
            password[1] = lowerCase[random.Next(lowerCase.Length)];
            password[2] = digits[random.Next(digits.Length)];
            password[3] = specialCharacters[random.Next(specialCharacters.Length)];

            for (int i = 4; i < length; i++)
            {
                password[i] = allCharacters[random.Next(allCharacters.Length)];
            }

            for (int i = 0; i < password.Length; i++)
            {
                int j = random.Next(password.Length);
                char temp = password[i];
                password[i] = password[j];
                password[j] = temp;
            }

            return new string(password);
        }
    }
}