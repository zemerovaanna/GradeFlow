using GradeFlowECTS.Helpers;

namespace GradeFlowTests
{
    public class InputValidatorTests
    {
        [Theory]
        [InlineData('А', true)]
        [InlineData('Я', true)]
        [InlineData('ё', false)]
        [InlineData('5', true)]
        [InlineData('Z', false)]
        public void IsValidRuNDigitCharacter_ValidateCorrectly(char character, bool expected)
        {
            var result = InputValidator.IsValidRuNDigitCharacter(character);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData('a', true)]
        [InlineData('Z', true)]
        [InlineData('7', true)]
        [InlineData('.', true)]
        [InlineData('@', true)]
        [InlineData('#', false)]
        public void IsValidMailCharacter_ValidateCorrectly(char character, bool expected)
        {
            var result = InputValidator.IsValidMailCharacter(character);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("'Ivanov'", "Ivanov")]
        [InlineData(" ivanov ", "Ivanov")]
        [InlineData("'ivanov", "Ivanov")]
        [InlineData("ivanov'", "Ivanov")]
        [InlineData("IVANOV'", "Ivanov")]
        [InlineData("iVANOV'", "Ivanov")]
        [InlineData("Admin@domain.com", "Admin@domain.com")]
        public void TrimApostrophesAndRemoveSpaces_NormalizeCorrectly(string input, string expected)
        {
            var result = InputValidator.TrimApostrophesAndRemoveSpaces(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Алексей", true)]
        [InlineData("О'Хара", true)]
        [InlineData("Ан-на", true)]
        [InlineData("-Иван", false)]
        [InlineData("Пётр-", false)]
        [InlineData("Ан--на", false)]
        [InlineData("О''Анна", false)]
        [InlineData("Чрезмерно''''''''''''''", false)]
        [InlineData("123", false)]
        public void IsValidName_ValidateCorrectly(string input, bool expected)
        {
            var result = InputValidator.IsValidName(input, out var _);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("test@example.com", true)]
        [InlineData("user.name+tag+sorting@example.com", true)]
        [InlineData("plainaddress", false)]
        [InlineData("@missingusername.com", false)]
        [InlineData("username@.com", false)]
        public void IsValidMail_ValidateCorrectly(string input, bool expected)
        {
            var result = InputValidator.IsValidMail(input, out var _);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Abcdef1!", true)]
        [InlineData("Short1!", false)]
        [InlineData("alllowercase1!", false)]
        [InlineData("ALLUPPERCASE1!", false)]
        [InlineData("NoDigits!", false)]
        [InlineData("NoSpecialChar1", false)]
        public void IsValidPassword_ValidateCorrectly(string input, bool expected)
        {
            var result = InputValidator.IsValidPassword(input, out var _);
            Assert.Equal(expected, result);
        }
    }
}