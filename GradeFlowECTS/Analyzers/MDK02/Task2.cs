using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task2
    {
        public static int TotalCriteria { get; set; } = 3;
        public static int MetCriteria { get; set; } = 0;
        public static List<string> CriteriaDetails { get; } = new();

        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MetCriteria = 0;
            CriteriaDetails.Clear();

            CheckGraphClass(root);
            CheckFunctionMethod(root);
            CheckGraphTests(root);

            return ($"{MetCriteria}/{TotalCriteria}", PrintResults());
        }

        private static void CheckGraphClass(SyntaxNode root)
        {
            var classNodes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            foreach (var classNode in classNodes)
            {
                var className = classNode.Identifier.Text.ToLower();
                if (className.Contains("graph") || className.Contains("function"))
                {
                    MetCriteria++;
                    CriteriaDetails.Add("✅ Найден класс, связанный с графиком функции");
                    return;
                }
            }

            CriteriaDetails.Add("❌ Класс, связанный с графиком функции, не найден");
        }

        private static void CheckFunctionMethod(SyntaxNode root)
        {
            var methodNodes = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var method in methodNodes)
            {
                var bodyText = method.Body?.ToString().ToLower() ?? "";

                if (
                    bodyText.Contains("1") &&
                    (bodyText.Contains("/(x-4)") ||
                     (bodyText.Contains("/") && bodyText.Contains("x") && bodyText.Contains("4")))
                   )
                {
                    MetCriteria++;
                    CriteriaDetails.Add("✅ Метод, вычисляющий значения функции y = 1 / (x - 4), найден");
                    return;
                }
            }

            CriteriaDetails.Add("❌ Метод вычисления значений функции не найден или не соответствует y = 1 / (x - 4)");
        }

        private static void CheckGraphTests(SyntaxNode root)
        {
            var testClasses = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Where(cls => cls.AttributeLists.SelectMany(a => a.Attributes)
                    .Any(attr => attr.Name.ToString().ToLower().Contains("test")) ||
                              cls.Identifier.Text.ToLower().Contains("test"));

            bool foundValidTest = false;

            foreach (var testClass in testClasses)
            {
                var methods = testClass.Members.OfType<MethodDeclarationSyntax>();
                foreach (var method in methods)
                {
                    var attrs = method.AttributeLists.SelectMany(a => a.Attributes)
                        .Select(attr => attr.Name.ToString().ToLower());

                    bool isFactOrTheory = attrs.Any(a => a.Contains("fact") || a.Contains("theory") || a.Contains("testmethod"));

                    bool containsAssert = method.DescendantNodes()
                        .OfType<InvocationExpressionSyntax>()
                        .Any(i => i.ToString().ToLower().Contains("assert"));

                    bool testsFunction = method.ToString().ToLower().Contains("/(x-4)") ||
                                         (method.ToString().ToLower().Contains("/") && method.ToString().ToLower().Contains("4"));

                    if (isFactOrTheory && containsAssert && testsFunction)
                    {
                        foundValidTest = true;
                        break;
                    }
                }

                if (foundValidTest)
                    break;
            }

            if (foundValidTest)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Найден автоматизированный тест метода вычисления значений функции");
            }
            else
            {
                CriteriaDetails.Add("❌ Автоматизированный тест метода вычисления значений функции не найден");
            }
        }

        public static string PrintResults()
        {
            string result = $"Выполнено критериев: {MetCriteria} из {TotalCriteria}\nДетали:\n";
            foreach (var detail in CriteriaDetails)
            {
                result += $"{detail}\n";
            }
            return result;
        }
    }
}