using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task5Analyzer
    {
        public static (string totalScore, string criteria) Analyze(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var compilation = CSharpCompilation.Create("Analysis")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddSyntaxTrees(tree);

            var root = tree.GetRoot();
            var semanticModel = compilation.GetSemanticModel(tree);

            ArrayAnalyzer.Analyze(root);
            CheckSpecificConditions(root, semanticModel);
            return ($"{ArrayAnalyzer.MetCriteria}/{ArrayAnalyzer.TotalCriteria+1}", ArrayAnalyzer.PrintResults());
        }

        private static void CheckSpecificConditions(SyntaxNode root, SemanticModel semanticModel)
        {
            bool hasArraySizeInput = HasArraySizeInput(root, semanticModel);
            bool hasArrayInitialization = HasArrayInitialization(root);
            bool hasArrayFilling = HasArrayFilling(root);
            bool hasComparison = HasRightNeighborComparison(root);
            bool hasIndexCollection = HasIndexCollection(root);
            bool hasCountOutput = HasCountOutput(root);
            bool hasAscendingOutput = HasAscendingOutput(root);
            bool hasOutputFormatting = HasOutputFormatting(root);
            bool hasLoopForComparison = HasLoopForComparison(root);
            bool hasArrayOutput = HasArrayOutput(root, semanticModel);

            UpdateCriteria(
                hasArraySizeInput,
                hasArrayInitialization,
                hasArrayFilling,
                hasComparison,
                hasIndexCollection,
                hasCountOutput,
                hasAscendingOutput,
                hasOutputFormatting,
                hasLoopForComparison,
                hasArrayOutput
            );
        }

        private static bool HasArraySizeInput(SyntaxNode root, SemanticModel semanticModel)
        {
            var variableDeclarations = root.DescendantNodes()
                .OfType<VariableDeclaratorSyntax>();

            foreach (var variable in variableDeclarations)
            {
                var initializer = variable.Initializer?.Value;
                if (initializer == null) continue;

                var symbol = semanticModel.GetSymbolInfo(initializer).Symbol;
                if (symbol == null) continue;

                if (symbol.Kind == SymbolKind.Method &&
                    symbol.ToDisplayString().Contains("Console.ReadLine"))
                {
                    return true;
                }
            }

            return false;
        }


        private static bool HasArrayInitialization(SyntaxNode root)
        {
            // Проверяем создание массива с размером из переменной или выражения
            return root.DescendantNodes()
                .OfType<ArrayCreationExpressionSyntax>()
                .Any(a => a.Type.RankSpecifiers[0].Sizes[0] is IdentifierNameSyntax ||
                         a.Type.RankSpecifiers[0].Sizes[0] is InvocationExpressionSyntax);
        }

        private static bool HasArrayFilling(SyntaxNode root)
        {
            // Более гибкая проверка заполнения массива
            return root.DescendantNodes()
                .OfType<ForStatementSyntax>()
                .Any(f => f.Statement.DescendantNodes()
                    .OfType<AssignmentExpressionSyntax>()
                    .Any(a => a.Left is ElementAccessExpressionSyntax) &&
                    f.Statement.DescendantNodes()
                    .OfType<InvocationExpressionSyntax>()
                    .Any(i => i.ToString().Contains("ReadLine")));
        }

        private static bool HasArrayOutput(SyntaxNode root, SemanticModel semanticModel)
        {
            var invocations = root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Where(i => i.ToString().Contains("Write") || i.ToString().Contains("WriteLine"));

            foreach (var invocation in invocations)
            {
                foreach (var argument in invocation.ArgumentList.Arguments)
                {
                    var symbol = semanticModel.GetSymbolInfo(argument.Expression).Symbol;
                    if (symbol is ILocalSymbol localSymbol &&
                        localSymbol.Type.TypeKind == TypeKind.Array)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool HasRightNeighborComparison(SyntaxNode root)
        {
            var binaryExpressions = root.DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .Where(b => b.IsKind(SyntaxKind.GreaterThanExpression));

            foreach (var expression in binaryExpressions)
            {
                if (expression.Left is ElementAccessExpressionSyntax left &&
                    expression.Right is ElementAccessExpressionSyntax right)
                {
                    if (left.Expression.ToString() == right.Expression.ToString())
                    {
                        var leftIndex = left.ArgumentList.Arguments.First().ToString();
                        var rightIndex = right.ArgumentList.Arguments.First().ToString();

                        if (rightIndex == $"{leftIndex} + 1" || rightIndex == $"{leftIndex}+1")
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static bool HasIndexCollection(SyntaxNode root)
        {
            // Более надежная проверка сохранения индексов
            return root.DescendantNodes()
                .OfType<ObjectCreationExpressionSyntax>()
                .Any(o => o.Type.ToString().Contains("List<int>")) &&
                root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Any(i => i.ToString().Contains(".Add(") &&
                         i.ArgumentList.Arguments.Count == 1);
        }

        private static bool HasCountOutput(SyntaxNode root)
        {
            // Проверяем вывод количества элементов (Count или индексы.Count)
            return root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Any(i => i.ToString().Contains("Write") &&
                         i.ArgumentList.Arguments.Any(a =>
                             a.ToString().Contains(".Count") ||
                             a.ToString().Contains("count")));
        }

        private static bool HasAscendingOutput(SyntaxNode root)
        {
            // Проверяем вывод индексов в порядке возрастания
            return root.DescendantNodes()
                .OfType<ForEachStatementSyntax>()
                .Any(f => f.Expression.ToString().Contains("indices") ||
                          f.Expression.ToString().Contains("indexes")) ||
                root.DescendantNodes()
                .OfType<ForStatementSyntax>()
                .Any(f => f.Statement.DescendantNodes()
                    .OfType<InvocationExpressionSyntax>()
                    .Any(i => i.ToString().Contains("Write")));
        }

        private static bool HasLoopForComparison(SyntaxNode root)
        {
            // Проверяем наличие цикла для сравнения соседей
            return root.DescendantNodes()
                .OfType<ForStatementSyntax>()
                .Any(f => f.Condition.ToString().Contains("<") &&
                         f.Statement.DescendantNodes()
                            .OfType<IfStatementSyntax>()
                            .Any(i => i.Condition.ToString().Contains(">")));
        }

        private static bool HasOutputFormatting(SyntaxNode root)
        {
            var writeLines = root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Where(i => i.ToString().Contains("WriteLine"));

            bool hasIndexOutput = writeLines.Any(w =>
                w.ArgumentList.Arguments.Any(a =>
                    a.ToString().IndexOf("индекс", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    a.ToString().IndexOf("номер", StringComparison.OrdinalIgnoreCase) >= 0));

            bool hasCountOutput = writeLines.Any(w =>
                w.ArgumentList.Arguments.Any(a =>
                    a.ToString().IndexOf("колич", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    a.ToString().IndexOf("всего", StringComparison.OrdinalIgnoreCase) >= 0));

            return hasIndexOutput && hasCountOutput;
        }

        private static void UpdateCriteria(params bool[] checks)
        {
            // Основные критерии (первые 8)
            ArrayAnalyzer.TotalCriteria = 8;
            ArrayAnalyzer.MetCriteria = checks.Take(8).Count(c => c);

            // Детализация результатов
            ArrayAnalyzer.CriteriaDetails.Clear();
            ArrayAnalyzer.CriteriaDetails.Add(checks[0]
                ? "✅ Ввод размерности массива выполнен"
                : "❌ Ввод размерности массива не выполнен");

            ArrayAnalyzer.CriteriaDetails.Add(checks[1]
                ? "✅ Инициализация массива выполнена"
                : "❌ Инициализация массива не выполнена");

            ArrayAnalyzer.CriteriaDetails.Add(checks[2]
                ? "✅ Заполнение массива выполнено"
                : "❌ Заполнение массива не выполнено");

            ArrayAnalyzer.CriteriaDetails.Add(checks[3]
                ? "✅ Сравнение с правым соседом выполнено"
                : "❌ Сравнение с правым соседом не выполнено");

            ArrayAnalyzer.CriteriaDetails.Add(checks[4]
                ? "✅ Сбор номеров элементов выполнен"
                : "❌ Сбор номеров элементов не выполнен");

            ArrayAnalyzer.CriteriaDetails.Add(checks[5]
                ? "✅ Подсчет количества элементов выполнен"
                : "❌ Подсчет количества элементов не выполнен");

            ArrayAnalyzer.CriteriaDetails.Add(checks[6]
                ? "✅ Вывод в порядке возрастания выполнен"
                : "❌ Вывод в порядке возрастания не выполнен");

            ArrayAnalyzer.CriteriaDetails.Add(checks[7]
                ? "✅ Форматированный вывод результатов выполнен"
                : "❌ Форматированный вывод результатов не выполнен");

            // Дополнительные проверки (не влияют на счет)
            if (checks.Length > 8)
            {
                ArrayAnalyzer.CriteriaDetails.Add(checks[8]
                    ? "● Цикл для сравнения элементов присутствует"
                    : "○ Цикл для сравнения элементов отсутствует");

                ArrayAnalyzer.CriteriaDetails.Add(checks[9]
                    ? "● Вывод массива выполнен"
                    : "○ Вывод массива не выполнен");
            }
        }
    }
}