using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task8
    {
        public static int MetCriteria { get; private set; } = 0;
        public static int TotalCriteria { get; } = 8;
        public static List<string> CriteriaDetails { get; } = new();

        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MetCriteria = 0;
            CriteriaDetails.Clear();

            var abClass = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(c => c.Members.OfType<FieldDeclarationSyntax>()
                    .Any(f => f.ToString().ToLower().Contains("a")) &&
                    c.Members.OfType<FieldDeclarationSyntax>()
                    .Any(f => f.ToString().ToLower().Contains("b")));

            if (abClass != null)
            {
                CriteriaDetails.Add("✅ Класс с полями A и B найден");
                CheckPrivateFields(abClass);
                CheckABMethods(abClass);
            }
            else
            {
                CriteriaDetails.Add("❌ Класс с полями A и B не найден");
            }

            CheckTestFrameworks(root);
            return ($"{MetCriteria}/{TotalCriteria}", PrintSummary());
        }

        private static void CheckPrivateFields(ClassDeclarationSyntax abClass)
        {
            var fields = abClass.Members.OfType<FieldDeclarationSyntax>();
            bool hasPrivateA = fields.Any(f => f.Modifiers.Any(m => m.Text == "private") &&
                                               f.ToString().ToLower().Contains("a"));
            bool hasPrivateB = fields.Any(f => f.Modifiers.Any(m => m.Text == "private") &&
                                               f.ToString().ToLower().Contains("b"));

            if (hasPrivateA && hasPrivateB)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Приватные поля A и B найдены");
            }
            else
            {
                CriteriaDetails.Add("❌ Приватные поля A и/или B отсутствуют");
            }
        }

        private static void CheckABMethods(ClassDeclarationSyntax abClass)
        {
            CheckMethod(abClass, "sum", "a + b", "Сумма", 2);
            CheckMethod(abClass, "power", "math.pow", "Степень", 3);
            CheckMethod(abClass, "divide", "/", "Частное", 4);
            CheckMethod(abClass, "print", "console.write", "Повторение A B раз", 5);
        }

        private static void CheckMethod(ClassDeclarationSyntax abClass, string idKey, string bodyHint, string desc, int critNum)
        {
            var method = abClass.Members.OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.Identifier.Text.ToLower().Contains(idKey) &&
                                     m.ToString().ToLower().Contains(bodyHint));

            if (method != null)
            {
                MetCriteria++;
                CriteriaDetails.Add($"✅ Метод '{desc}' реализован");
            }
            else
            {
                CriteriaDetails.Add($"❌ Метод '{desc}' не найден или не реализован корректно");
            }
        }

        private static void CheckTestFrameworks(SyntaxNode root)
        {
            var testClass = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(c => c.AttributeLists.ToString().ToLower().Contains("test"));

            if (testClass != null)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Найден тестовый класс");
            }
            else
            {
                CriteriaDetails.Add("❌ Тестовый класс не найден");
                return;
            }

            var testMethods = testClass.Members.OfType<MethodDeclarationSyntax>()
                .Where(m => m.AttributeLists.Any(attr =>
                attr.ToString().ToLower().Contains("testmethod") ||
                attr.ToString().ToLower().Contains("fact") ||
                attr.ToString().ToLower().Contains("theory") ||
                attr.ToString().ToLower().Contains("test")))
                .ToList();


            if (testMethods.Count > 0)
            {
                MetCriteria++;
                CriteriaDetails.Add($"✅ Найдено тестов: {testMethods.Count}");
            }
            else
            {
                CriteriaDetails.Add("❌ Не найдены тестовые методы");
            }

            // Проверка охвата методов
            var tested = new[] { "sum", "power", "divide", "print" };
            bool allCovered = tested.All(mName =>
                testMethods.Any(tm => tm.ToString().ToLower().Contains(mName)));

            if (allCovered)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Все методы класса протестированы");
            }
            else
            {
                CriteriaDetails.Add("❌ Не все методы протестированы");
            }
        }

        private static string PrintSummary()
        {
            var summary = $"Критерии выполнены: {MetCriteria} из {TotalCriteria}\nДетали:\n";
            foreach (var line in CriteriaDetails)
                summary += line + "\n";

            return summary;
        }
    }
}