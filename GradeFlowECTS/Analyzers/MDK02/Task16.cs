using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task16
    {
        public static int TotalCriteria { get; set; } = 4;
        public static int MetCriteria { get; set; } = 0;
        public static List<string> CriteriaDetails { get; } = new();

        public static string AnalyzeFromText(string fullText)
        {
            var tree = CSharpSyntaxTree.ParseText(fullText);
            var root = tree.GetRoot();
            return Analyze(new List<SyntaxNode> { root });
        }

        public static string Analyze(List<SyntaxNode> roots)
        {
            MetCriteria = 0;
            CriteriaDetails.Clear();

            CheckInt2DArrayFieldOrProperty(roots);
            CheckInitializationOrInputMethod(roots);
            CheckPrintMethod(roots);
            CheckSumByRowsMethod(roots);
            CheckTests(roots);

            return PrintResults();
        }

        private static void CheckInt2DArrayFieldOrProperty(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var classNodes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

                foreach (var classNode in classNodes)
                {
                    var has2DIntArray = classNode.Members.Any(m =>
                        (m is PropertyDeclarationSyntax p &&
                         p.Type is ArrayTypeSyntax arrType &&
                         arrType.ElementType.ToString().ToLower() == "int" &&
                         arrType.RankSpecifiers.Any(rs => rs.Rank == 2))
                        ||
                        (m is FieldDeclarationSyntax f &&
                         f.Declaration.Type is ArrayTypeSyntax arrTypeF &&
                         arrTypeF.ElementType.ToString().ToLower() == "int" &&
                         arrTypeF.RankSpecifiers.Any(rs => rs.Rank == 2)));

                    if (has2DIntArray)
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден класс с двумерным целочисленным массивом (поле или свойство).");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Двумерный целочисленный массив не найден.");
        }

        private static void CheckInitializationOrInputMethod(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

                foreach (var method in methods)
                {
                    var bodyText = method.ToString().ToLower();

                    bool initializesArray = bodyText.Contains("new int[") || bodyText.Contains("array") || bodyText.Contains("input") || bodyText.Contains("read");

                    if ((method.Identifier.Text.ToLower().Contains("init") || method.Identifier.Text.ToLower().Contains("input") || method.Identifier.Text.ToLower().Contains("read")) && initializesArray)
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден метод инициализации или ввода элементов массива.");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Метод инициализации или ввода элементов массива не найден.");
        }

        private static void CheckPrintMethod(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

                foreach (var method in methods)
                {
                    var bodyText = method.ToString().ToLower();

                    if ((method.Identifier.Text.ToLower().Contains("print") || method.Identifier.Text.ToLower().Contains("display") || method.Identifier.Text.ToLower().Contains("write")) &&
                        (bodyText.Contains("console.writeline") || bodyText.Contains("print") || bodyText.Contains("write")))
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден метод вывода элементов массива.");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Метод вывода элементов массива не найден.");
        }

        private static void CheckSumByRowsMethod(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

                foreach (var method in methods)
                {
                    var bodyText = method.ToString().ToLower();

                    bool sumsRows = bodyText.Contains("sum") && bodyText.Contains("rows") || bodyText.Contains("sum") && bodyText.Contains("length");

                    if ((method.Identifier.Text.ToLower().Contains("sum") || method.Identifier.Text.ToLower().Contains("rowsum")) && sumsRows)
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден метод нахождения суммы по строкам.");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Метод нахождения суммы по строкам не найден.");
        }

        private static void CheckTests(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

                foreach (var method in methodDeclarations)
                {
                    var attributes = method.AttributeLists.SelectMany(al => al.Attributes).ToList();
                    var hasTestAttribute = attributes.Any(attr =>
                    {
                        var name = attr.Name.ToString().ToLower();
                        return name.Contains("test") || name.Contains("fact") || name.Contains("theory");
                    });

                    var hasAssert = method.DescendantNodes()
                        .OfType<InvocationExpressionSyntax>()
                        .Any(inv => inv.Expression.ToString().ToLower().Contains("assert"));

                    if (hasTestAttribute && hasAssert)
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден тест с атрибутом и Assert.");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Тесты не найдены или некорректны.");
        }

        public static string PrintResults()
        {
            string result = $"Выполнено критериев: {MetCriteria} из {TotalCriteria}\nДетали:\n";
            foreach (var detail in CriteriaDetails)
            {
                result += detail + "\n";
            }
            return result;
        }
    }
}