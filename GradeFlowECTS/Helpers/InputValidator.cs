using System.Text.RegularExpressions;

namespace GradeFlowECTS.Helpers
{
    /// <summary>
    /// Предоставляет вспомогательные методы для валидации пользовательского ввода, таких как имя, пароль и электронная почта.
    /// </summary>
    public static class InputValidator
    {
        /// <summary>
        /// Проверяет, является ли символ допустимым русским буквенно-цифровым символом (русские буквы и цифры).
        /// </summary>
        /// <param name="character">Проверяемый символ.</param>
        /// <returns>True, если символ допустим; иначе false.</returns>
        public static bool IsValidRuNDigitCharacter(char character)
        {
            string validCharacters = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ0123456789";
            return validCharacters.Contains(character);
        }

        /// <summary>
        /// Проверяет, является ли символ допустимым символом для ввода электронной почты.
        /// </summary>
        /// <param name="character">Проверяемый символ.</param>
        /// <returns>True, если символ допустим; иначе false.</returns>
        public static bool IsValidMailCharacter(char character)
        {
            string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._-@";
            return validCharacters.Contains(character);
        }

        /// <summary>
        /// Удаляет пробелы и внешние апострофы из строки, при необходимости форматирует как имя.
        /// </summary>
        /// <param name="text">Входная строка для обработки.</param>
        /// <returns>Обработанная строка без пробелов и лишних апострофов.</returns>
        public static string TrimApostrophesAndRemoveSpaces(string text)
        {
            text = text.Replace(" ", "");

            if (text.StartsWith("\'")) text = text.Substring(1);
            if (text.EndsWith("\'")) text = text.Substring(0, text.Length - 1);

            if (!IsValidMail(text, out var _))
            {
                text = char.ToUpper(text[0]) + text.Substring(1).ToLower();
            }

            return text;
        }

        /// <summary>
        /// Проверяет корректность имени согласно заданным правилам (русские буквы, апострофы, дефисы и др.).
        /// </summary>
        /// <param name="input">Проверяемое имя.</param>
        /// <param name="error">Сообщение об ошибке, если валидация не пройдена.</param>
        /// <returns>True, если имя корректно; иначе false.</returns>
        public static bool IsValidName(string input, out string error)
        {
            error = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                error = "Имя не может быть пустым.";
                return false;
            }

            if (!Regex.IsMatch(input, @"^[А-Яа-я'-]+$"))
            {
                error = "Имя может содержать только русские буквы, дефисы и апострофы.";
                return false;
            }

            if (input[0] == '-' || input[input.Length - 1] == '-')
            {
                error = "Имя не может начинаться или заканчиваться дефисом.";
                return false;
            }

            int hyphenCount = input.Count(c => c == '-');
            if (hyphenCount > 1)
            {
                error = "Имя может содержать не более одного дефиса.";
                return false;
            }

            int apostropheCount = input.Count(c => c == '\'');
            double apostrophePercentage = (double)apostropheCount / input.Length * 100;
            if (apostrophePercentage > 20)
            {
                error = "Слишком много апострофов в имени.";
                return false;
            }

            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] == '\'' && input[i - 1] == '\'')
                {
                    error = "Имя не может содержать подряд идущие апострофы.";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Проверяет корректность электронной почты по стандартному формату.
        /// </summary>
        /// <param name="mail">Проверяемый адрес электронной почты.</param>
        /// <param name="error">Сообщение об ошибке, если валидация не пройдена.</param>
        /// <returns>True, если почта корректна; иначе false.</returns>
        public static bool IsValidMail(string mail, out string error)
        {
            error = null;

            if (string.IsNullOrWhiteSpace(mail))
            {
                error = "Почта не может быть пустой.";
                return false;
            }

            if (!Regex.IsMatch(mail, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                error = "Некорректный формат электронной почты.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Проверяет надёжность пароля по заданным критериям (длина, регистр, цифры, спецсимволы).
        /// </summary>
        /// <param name="password">Проверяемый пароль.</param>
        /// <param name="error">Сообщение об ошибке, если валидация не пройдена.</param>
        /// <returns>True, если пароль надёжный; иначе false.</returns>
        public static bool IsValidPassword(string password, out string error)
        {
            error = null;

            if (string.IsNullOrEmpty(password))
            {
                error = "Пароль не может быть пустым.";
                return false;
            }

            if (password.Length < 8)
            {
                error = "Пароль должен содержать минимум 8 символов.";
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                error = "Пароль должен содержать хотя бы одну заглавную букву.";
                return false;
            }

            if (!password.Any(char.IsLower))
            {
                error = "Пароль должен содержать хотя бы одну строчную букву.";
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                error = "Пароль должен содержать хотя бы одну цифру.";
                return false;
            }

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                error = "Пароль должен содержать хотя бы один специальный символ.";
                return false;
            }

            return true;
        }
    }
}