using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task6Analyzer
    {
        public static (string totalScore, string criteria) Analyze(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var compilation = CSharpCompilation.Create("Analysis")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddSyntaxTrees(tree);

            var root = tree.GetRoot();
            var semanticModel = compilation.GetSemanticModel(tree);

            bool hasArraySizeInput = HasArraySizeInput(root, semanticModel);
            bool hasArrayInitialization = HasArrayInitialization(root);
            bool hasArrayFilling = HasArrayFilling(root);
            bool hasLeftNeighborComparison = HasLeftNeighborComparison(root);
            bool hasIndexCollection = HasIndexCollection(root);
            bool hasCountOutput = HasCountOutput(root);
            bool hasDescendingOutput = HasDescendingOutput(root);
            bool hasForbiddenSortUsage = HasForbiddenSortUsage(root);

            var results = new List<string>
            {
                hasArraySizeInput
                    ? "✅ Ввод размерности массива выполнен"
                    : "❌ Ввод размерности массива не выполнен",

                hasArrayInitialization
                    ? "✅ Инициализация массива выполнена"
                    : "❌ Инициализация массива не выполнена",

                hasArrayFilling
                    ? "✅ Заполнение массива выполнено"
                    : "❌ Заполнение массива не выполнено",

                hasLeftNeighborComparison
                    ? "✅ Сравнение с левым соседом выполнено"
                    : "❌ Сравнение с левым соседом не выполнено",

                hasIndexCollection
                    ? "✅ Сбор номеров элементов выполнен"
                    : "❌ Сбор номеров элементов не выполнен",

                hasCountOutput
                    ? "✅ Подсчёт количества выполнен"
                    : "❌ Подсчёт количества не выполнен",

                hasDescendingOutput
                    ? "✅ Вывод в порядке убывания выполнен"
                    : "❌ Вывод в порядке убывания не выполнен",

                hasForbiddenSortUsage
                    ? "❌ Использован метод сортировки (запрещено)"
                    : "✅ Не используется запрещённая сортировка"
            };

            return ($"{ArrayAnalyzer.MetCriteria}/{ArrayAnalyzer.TotalCriteria}", string.Join(Environment.NewLine, results));
        }

        private static bool HasArraySizeInput(SyntaxNode root, SemanticModel semanticModel)
        {
            return root.DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Any(v =>
                {
                    var init = v.Initializer?.Value;
                    if (init == null) return false;

                    var symbol = semanticModel.GetSymbolInfo(init).Symbol;
                    return symbol?.ToDisplayString().Contains("Console.ReadLine") == true;
                });
        }

        private static bool HasArrayInitialization(SyntaxNode root)
        {
            return root.DescendantNodes()
                .OfType<ArrayCreationExpressionSyntax>()
                .Any();
        }

        private static bool HasArrayFilling(SyntaxNode root)
        {
            return root.DescendantNodes()
                .OfType<ForStatementSyntax>()
                .Any(f =>
                    f.Statement.DescendantNodes().OfType<AssignmentExpressionSyntax>()
                        .Any(a => a.Left is ElementAccessExpressionSyntax) &&
                    f.Statement.DescendantNodes().OfType<InvocationExpressionSyntax>()
                        .Any(i => i.ToString().Contains("ReadLine"))
                );
        }

        private static bool HasLeftNeighborComparison(SyntaxNode root)
        {
            return root.DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .Any(b =>
                    b.IsKind(SyntaxKind.GreaterThanExpression) &&
                    b.Left is ElementAccessExpressionSyntax left &&
                    b.Right is ElementAccessExpressionSyntax right &&
                    left.Expression.ToString() == right.Expression.ToString() &&
                    right.ArgumentList.Arguments.First().ToString() == left.ArgumentList.Arguments.First() + " - 1"
                );
        }

        private static bool HasIndexCollection(SyntaxNode root)
        {
            return root.DescendantNodes()
                .OfType<ObjectCreationExpressionSyntax>()
                .Any(o => o.Type.ToString().Contains("List<int>")) &&
                root.DescendantNodes()
                    .OfType<InvocationExpressionSyntax>()
                    .Any(i => i.ToString().Contains(".Add("));
        }

        private static bool HasCountOutput(SyntaxNode root)
        {
            return root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Any(i =>
                    i.ToString().Contains("Write") &&
                    i.ArgumentList.Arguments.Any(a =>
                        a.ToString().Contains(".Count") ||
                        a.ToString().Contains("count"))
                );
        }

        private static bool HasDescendingOutput(SyntaxNode root)
        {
            // Проверка на цикл с обратным счётчиком и вывод внутри него
            return root.DescendantNodes()
                .OfType<ForStatementSyntax>()
                .Any(f =>
                    f.Condition.ToString().Contains(">=") &&
                    f.Statement.DescendantNodes().OfType<InvocationExpressionSyntax>()
                        .Any(i => i.ToString().Contains("Write"))
                );
        }

        private static bool HasForbiddenSortUsage(SyntaxNode root)
        {
            // Запрещённые методы: Sort, OrderBy, Reverse
            return root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Any(i =>
                    i.ToString().Contains(".Sort") ||
                    i.ToString().Contains(".OrderBy") ||
                    i.ToString().Contains(".Reverse"));
        }
    }
}