using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task11
    {
        public static int TotalCriteria => 2;
        public static int MetCriteria { get; private set; }
        public static List<string> Details { get; } = new();

        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MetCriteria = 0;
            Details.Clear();

            var classExists = root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Any(cls => cls.Identifier.Text.ToLower().Contains("счет") || cls.Identifier.Text.ToLower().Contains("schet"));

            if (classExists)
            {
                MetCriteria++;
                Details.Add("✅ Класс 'Счет' найден");
            }
            else
            {
                Details.Add("❌ Класс 'Счет' не найден");
            }

            var hasSumDigitsLogic = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Any(m => m.ToString().ToLower().Contains("char") &&
                          m.ToString().ToLower().Contains("isdigit") &&
                          (m.ToString().Contains("+=") || m.ToString().Contains("sum")));

            if (hasSumDigitsLogic)
            {
                MetCriteria++;
                Details.Add("✅ Найден метод суммирования цифр в строке");
            }
            else
            {
                Details.Add("❌ Метод суммирования цифр не найден");
            }

            return ($"{MetCriteria}/{TotalCriteria}", Print());
        }

        private static string Print()
        {
            return $"Задание 11: выполнено {MetCriteria} из {TotalCriteria}\n" + string.Join("\n", Details);
        }
    }
}