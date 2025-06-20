using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task7Analyzer
    {
        public static (string totalScore, string criteria) Analyze(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var compilation = CSharpCompilation.Create("Analysis")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(Console).Assembly.Location))
                .AddSyntaxTrees(tree);

            var semanticModel = compilation.GetSemanticModel(tree);
            var root = tree.GetRoot();

            var criteriaDetails = new List<string>();
            const int totalCriteria = 8;
            int metCriteria = 0;

            void Add(bool result, string success, string fail)
            {
                criteriaDetails.Add(result ? $"✅ {success}" : $"❌ {fail}");
                if (result) metCriteria++;
            }

            Add(HasArraySizeInput(root, semanticModel),
                "Ввод размера массива обнаружен",
                "Не обнаружен ввод размера массива");

            Add(HasArrayInitialization(root),
                "Инициализация массива обнаружена",
                "Инициализация массива не обнаружена");

            Add(HasArrayFilling(root, semanticModel),
                "Заполнение массива с клавиатуры обнаружено",
                "Заполнение массива не обнаружено");

            Add(HasBubbleSort(root),
                "Пузырьковая сортировка обнаружена",
                "Пузырьковая сортировка не обнаружена");

            Add(HasElementSwap(root),
                "Обмен элементов обнаружен",
                "Обмен элементов не обнаружен");

            Add(HasCorrectInnerLoop(root),
                "Корректные границы внутреннего цикла",
                "Некорректные границы внутреннего цикла");

            Add(HasPassOutput(root),
                "Вывод после каждого прохода обнаружен",
                "Вывод после каждого прохода не обнаружен");

            Add(!UsesBuiltInSortingMethods(root),
                "Используется ручная сортировка",
                "Обнаружены встроенные методы сортировки");

            return ($"{metCriteria}/{totalCriteria+1}", $"Выполнено критериев: {metCriteria} из {totalCriteria}\nДетали:\n{string.Join("\n", criteriaDetails)}");
        }

        private static bool HasArraySizeInput(SyntaxNode root, SemanticModel model)
        {
            return root.DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Any(v =>
                {
                    var init = v.Initializer?.Value;
                    if (init == null) return false;

                    var symbol = model.GetSymbolInfo(init).Symbol as IMethodSymbol;
                    return init.ToString().Contains("ReadLine()") &&
                           (init.ToString().Contains("Parse") || init.ToString().Contains("ToInt32"));
                });
        }

        private static bool HasArrayInitialization(SyntaxNode root)
        {
            return root.DescendantNodes()
                .OfType<ArrayCreationExpressionSyntax>()
                .Any(a => a.Type.ElementType.ToString() == "int");
        }

        private static bool HasArrayFilling(SyntaxNode root, SemanticModel model)
        {
            return root.DescendantNodes()
                .OfType<ForStatementSyntax>()
                .Any(f => f.Statement.DescendantNodes()
                    .OfType<AssignmentExpressionSyntax>()
                    .Any(a =>
                        a.Left is ElementAccessExpressionSyntax &&
                        a.Right is InvocationExpressionSyntax invocation &&
                        (invocation.ToString().Contains("ReadLine()") ||
                         invocation.ToString().Contains("ToInt32") ||
                         invocation.ToString().Contains("Parse"))));
        }

        private static bool HasBubbleSort(SyntaxNode root)
        {
            var forLoops = root.DescendantNodes().OfType<ForStatementSyntax>().ToList();
            foreach (var outer in forLoops)
            {
                var innerLoops = outer.Statement.DescendantNodes().OfType<ForStatementSyntax>();
                foreach (var inner in innerLoops)
                {
                    var ifs = inner.Statement.DescendantNodes().OfType<IfStatementSyntax>();
                    foreach (var ifStmt in ifs)
                    {
                        if (ifStmt.Condition is BinaryExpressionSyntax binary &&
                            binary.OperatorToken.IsKind(SyntaxKind.GreaterThanToken) &&
                            binary.Left is ElementAccessExpressionSyntax &&
                            binary.Right is ElementAccessExpressionSyntax)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static bool HasElementSwap(SyntaxNode root)
        {
            return root.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any(ifStmt =>
                {
                    var assigns = ifStmt.Statement.DescendantNodes().OfType<AssignmentExpressionSyntax>().ToList();
                    return assigns.Count >= 2 || ifStmt.ToString().Contains("Swap(");
                });
        }

        private static bool HasCorrectInnerLoop(SyntaxNode root)
        {
            return root.DescendantNodes()
                .OfType<ForStatementSyntax>()
                .Any(inner =>
                {
                    var condition = inner.Condition?.ToString();
                    return condition != null &&
                           (condition.Contains("-i") || condition.Contains("- i")) &&
                           condition.Contains("<");
                });
        }

        private static bool HasPassOutput(SyntaxNode root)
        {
            return root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Any(i =>
                {
                    var text = i.ToString();
                    return text.Contains("Write") &&
                           (text.Contains("проход") || text.Contains("pass") || text.Contains("этап") || text.Contains("+"));
                });
        }

        private static bool UsesBuiltInSortingMethods(SyntaxNode root)
        {
            return root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Any(i => i.ToString().Contains("Array.Sort") ||
                          i.ToString().Contains("OrderBy") ||
                          i.ToString().Contains("ThenBy"));
        }
    }
}
