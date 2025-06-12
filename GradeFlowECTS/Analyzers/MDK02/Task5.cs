using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task5
    {
        public static int TotalCriteria { get; set; } = 4;
        public static int MetCriteria { get; set; } = 0;
        public static List<string> CriteriaDetails { get; } = new();

        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MetCriteria = 0;
            CriteriaDetails.Clear();

            CheckGraphClass(root);
            CheckFunctionMethod(root);
            CheckSegmentUsage(root);
            CheckTestsExist(root);

            return ($"{MetCriteria}/{TotalCriteria}", PrintResults());
        }

        private static void CheckGraphClass(SyntaxNode root)
        {
            var classNodes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach (var cls in classNodes)
            {
                var name = cls.Identifier.Text.ToLower();
                if (name.Contains("graph") || name.Contains("граф"))
                {
                    MetCriteria++;
                    CriteriaDetails.Add("✅ Найден класс, связанный с графиком");
                    return;
                }
            }
            CriteriaDetails.Add("❌ Класс, связанный с графиком, не найден");
        }

        private static void CheckFunctionMethod(SyntaxNode root)
        {
            var methodNodes = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (var method in methodNodes)
            {
                var methodText = method.ToString().ToLower();
                if (
                    methodText.Contains("1/") &&
                    (methodText.Contains("/x") || methodText.Contains("/ x") || methodText.Contains("1.0 /"))
                )
                {
                    MetCriteria++;
                    CriteriaDetails.Add("✅ Найден метод, вычисляющий значения функции y = 1/x");
                    return;
                }
            }

            CriteriaDetails.Add("❌ Метод, вычисляющий y = 1/x, не найден");
        }

        private static void CheckSegmentUsage(SyntaxNode root)
        {
            var methodNodes = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (var method in methodNodes)
            {
                var text = method.ToString().ToLower();
                if (
                    (text.Contains("from") && text.Contains("to")) ||
                    text.Contains("c") && text.Contains("d") && text.Contains("for") ||
                    text.Contains("range") || text.Contains("start") && text.Contains("end")
                )
                {
                    MetCriteria++;
                    CriteriaDetails.Add("✅ Обнаружена работа с отрезком [C; D]");
                    return;
                }
            }

            CriteriaDetails.Add("❌ Работа с отрезком [C; D] не обнаружена");
        }

        private static void CheckTestsExist(SyntaxNode root)
        {
            var testClasses = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Where(cls =>
                    cls.AttributeLists.SelectMany(a => a.Attributes)
                        .Any(attr =>
                            attr.Name.ToString().ToLower().Contains("test")) ||
                    cls.Identifier.Text.ToLower().Contains("test"));

            foreach (var testClass in testClasses)
            {
                var methods = testClass.Members.OfType<MethodDeclarationSyntax>();
                foreach (var method in methods)
                {
                    var methodText = method.ToString().ToLower();
                    bool hasAssert = methodText.Contains("assert");
                    bool hasRangeOrValues = methodText.Contains("c") || methodText.Contains("d") || methodText.Contains("x") || methodText.Contains("1/");

                    if (hasAssert && hasRangeOrValues)
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден тест, проверяющий входные данные или поведение функции");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Не найдены тесты, проверяющие входные данные или поведение функции");
        }

        private static string PrintResults()
        {
            var result = $"Выполнено критериев: {MetCriteria} из {TotalCriteria}\nДетали:\n";
            foreach (var detail in CriteriaDetails)
                result += detail + "\n";
            return result;
        }
    }
}