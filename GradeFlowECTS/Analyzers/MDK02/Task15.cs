using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task15
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

            CheckStringClassStructure(roots);
            CheckSplitMethod(roots);
            CheckWordCountMethod(roots);
            CheckContainsMethod(roots);
            CheckTests(roots);

            return PrintResults();
        }

        private static void CheckStringClassStructure(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var classNodes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

                foreach (var classNode in classNodes)
                {
                    var hasStringFieldOrProperty = classNode.Members.Any(m =>
                        (m is PropertyDeclarationSyntax p && p.Type.ToString().ToLower().Contains("string")) ||
                        (m is FieldDeclarationSyntax f && f.Declaration.Type.ToString().ToLower().Contains("string")));

                    if (hasStringFieldOrProperty)
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден класс, содержащий строковое поле или свойство.");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Класс со строковым полем или свойством не найден.");
        }

        private static void CheckSplitMethod(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

                foreach (var method in methods)
                {
                    var bodyText = method.ToString().ToLower();
                    if ((method.Identifier.Text.ToLower().Contains("split") || method.Identifier.Text.ToLower().Contains("words")) &&
                        bodyText.Contains(".split"))
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден метод, разбивающий строку на слова.");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Метод для разбиения строки на слова не найден.");
        }

        private static void CheckWordCountMethod(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

                foreach (var method in methods)
                {
                    var bodyText = method.ToString().ToLower();
                    if ((method.Identifier.Text.ToLower().Contains("count") || method.Identifier.Text.ToLower().Contains("words")) &&
                        (bodyText.Contains(".split") || bodyText.Contains(".length") || bodyText.Contains("count(")))
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден метод подсчёта количества слов.");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Метод подсчёта количества слов не найден.");
        }

        private static void CheckContainsMethod(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

                foreach (var method in methods)
                {
                    var bodyText = method.ToString().ToLower();
                    if ((method.Identifier.Text.ToLower().Contains("contains") || method.Identifier.Text.ToLower().Contains("has") || method.Identifier.Text.ToLower().Contains("check")) &&
                        bodyText.Contains(".contains"))
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден метод проверки наличия слова в строке.");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Метод проверки наличия слова в строке не найден.");
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