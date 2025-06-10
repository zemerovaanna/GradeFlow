using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task14
    {
        public static int TotalCriteria { get; set; } = 5;
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

            CheckOperationClassStructure(roots);
            CheckCalculationMethod(roots);
            CheckMaxMethod(roots);
            CheckFileWriteMethod(roots);
            CheckTests(roots);

            return PrintResults();
        }

        private static void CheckOperationClassStructure(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var classNodes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
                foreach (var classNode in classNodes)
                {
                    var allMembers = classNode.Members;

                    var numericMembers = allMembers
                        .Where(m =>
                            (m is PropertyDeclarationSyntax p &&
                                (p.Type.ToString().ToLower().Contains("double") || p.Type.ToString().ToLower().Contains("float"))) ||
                            (m is FieldDeclarationSyntax f &&
                                (f.Declaration.Type.ToString().ToLower().Contains("double") || f.Declaration.Type.ToString().ToLower().Contains("float"))))
                        .ToList();

                    var operationMembers = allMembers
                        .Where(m =>
                            (m is PropertyDeclarationSyntax p &&
                                (p.Type.ToString().ToLower().Contains("string") || p.Type.ToString().ToLower().Contains("char")) &&
                                p.Identifier.Text.ToLower().Contains("operation")) ||
                            (m is FieldDeclarationSyntax f &&
                                (f.Declaration.Type.ToString().ToLower().Contains("string") || f.Declaration.Type.ToString().ToLower().Contains("char")) &&
                                f.Declaration.Variables.Any(v => v.Identifier.Text.ToLower().Contains("operation"))))
                        .ToList();

                    if (numericMembers.Count >= 2 && operationMembers.Count >= 1)
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден класс с двумя числовыми переменными и полем операции (свойства или поля).");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Не найден корректный класс с двумя числовыми переменными и полем операции.");
        }

        private static void CheckCalculationMethod(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (var method in methods)
                {
                    var methodName = method.Identifier.Text.ToLower();
                    var methodText = method.ToString().ToLower();

                    if ((methodName.Contains("compute") || methodName.Contains("calculate")) &&
                        (methodText.Contains("+") || methodText.Contains("-") ||
                         methodText.Contains("*") || methodText.Contains("/") || methodText.Contains("math.pow")))
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден метод вычисления результата с использованием арифметических операций.");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Метод вычисления результата не найден.");
        }

        private static void CheckMaxMethod(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (var method in methods)
                {
                    if (method.Identifier.Text.ToLower().Contains("max") &&
                        method.ToString().ToLower().Contains("math.max"))
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден метод поиска максимального значения из двух переменных.");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Метод поиска максимального значения не найден.");
        }

        private static void CheckFileWriteMethod(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (var method in methods)
                {
                    var text = method.ToString().ToLower();
                    if ((method.Identifier.Text.ToLower().Contains("save") || method.Identifier.Text.ToLower().Contains("write")) &&
                        (text.Contains("file.write") || text.Contains("file.append")))
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден метод записи в файл.");
                        return;
                    }
                }
            }

            CriteriaDetails.Add("❌ Метод записи в файл не найден.");
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
