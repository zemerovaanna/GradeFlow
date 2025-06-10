using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task12
    {
        public static int TotalCriteria => 3;
        public static int MetCriteria { get; private set; }
        public static List<string> Details { get; } = new();

        public static string Analyze(SyntaxNode root)
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

            var hasFileRead = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Any(m => m.ToString().ToLower().Contains("file.read") ||
                          m.ToString().ToLower().Contains("streamreader"));

            if (hasFileRead)
            {
                MetCriteria++;
                Details.Add("✅ Найдено чтение из файла");
            }
            else
            {
                Details.Add("❌ Чтение из файла не найдено");
            }

            var hasProductLogic = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Any(m => m.ToString().ToLower().Contains("char") &&
                          m.ToString().ToLower().Contains("isdigit") &&
                          (m.ToString().Contains("*=") || m.ToString().ToLower().Contains("product")));

            if (hasProductLogic)
            {
                MetCriteria++;
                Details.Add("✅ Найден метод умножения цифр");
            }
            else
            {
                Details.Add("❌ Метод умножения цифр не найден");
            }

            return Print();
        }

        private static string Print()
        {
            return $"Задание 12: выполнено {MetCriteria} из {TotalCriteria}\n" + string.Join("\n", Details);
        }
    }
}