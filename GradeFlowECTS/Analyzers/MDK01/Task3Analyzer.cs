using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task3Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            ArrayAnalyzer.Analyze(root);
            CheckSpecificConditions(root);
            return ($"{ArrayAnalyzer.MetCriteria}/{ArrayAnalyzer.TotalCriteria}", ArrayAnalyzer.PrintResults());
        }

        private static void CheckSpecificConditions(SyntaxNode root)
        {
            // Проверка ввода K и L.
            bool hasKLInput = CheckKLInput(root);

            // Проверка условия 1 < K ≤ L ≤ N.
            bool hasKLCondition = CheckKLCondition(root);

            // Проверка вычисления суммы с исключением элементов K..L.
            bool hasSumCalculation = CheckSumCalculation(root);

            if (hasKLInput)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Ввод K и L выполнен");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Ввод K и L не выполнен");
                ArrayAnalyzer.TotalCriteria++;
            }

            if (hasKLCondition)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Проверка условия 1 < K ≤ L ≤ N выполнен");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Проверка условия 1 < K ≤ L ≤ N не выполнен");
                ArrayAnalyzer.TotalCriteria++;
            }

            if (hasSumCalculation)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Вычисление суммы с исключением элементов K..L выполнен");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Вычисление суммы с исключением элементов K..L не выполнен");
                ArrayAnalyzer.TotalCriteria++;
            }
        }

        private static bool CheckKLInput(SyntaxNode root)
        {
            var inputK = root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Where(i => i.Expression.ToString().Contains("ReadLine") ||
                            i.Expression.ToString().Contains("Read") ||
                            i.Expression.ToString().Contains("Parse"))
                .Where(i => i.Ancestors().OfType<VariableDeclarationSyntax>()
                    .Any(v => v.Variables.Any(v => v.Identifier.Text == "K" ||
                                                 v.Identifier.Text == "k")));

            var inputL = root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Where(i => i.Expression.ToString().Contains("ReadLine") ||
                            i.Expression.ToString().Contains("Read") ||
                            i.Expression.ToString().Contains("Parse"))
                .Where(i => i.Ancestors().OfType<VariableDeclarationSyntax>()
                    .Any(v => v.Variables.Any(v => v.Identifier.Text == "L" ||
                                                 v.Identifier.Text == "l")));

            return inputK.Any() && inputL.Any();
        }

        private static bool CheckKLCondition(SyntaxNode root)
        {
            return root.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any(i =>
                i.Condition.ToString().Contains("K > 1") ||
                i.Condition.ToString().Contains("K <= L") ||
                i.Condition.ToString().Contains("L <= N") ||
                i.Condition.ToString().Contains("1 < K") ||
                i.Condition.ToString().Contains("K ≤ L") ||
                i.Condition.ToString().Contains("L ≤ N") ||
                i.Condition.ToString().Contains("k > 1") ||
                i.Condition.ToString().Contains("k <= l") ||
                i.Condition.ToString().Contains("l <= n") ||
                i.Condition.ToString().Contains("1 < k") ||
                i.Condition.ToString().Contains("k ≤ l") ||
                i.Condition.ToString().Contains("l ≤ n"));
        }

        private static bool CheckSumCalculation(SyntaxNode root)
        {
            // Ищем цикл с условием исключения элементов K..L.
            var loops = root.DescendantNodes()
                .OfType<ForStatementSyntax>()
                .Where(f =>
                f.Condition.ToString().Contains("i < K") ||
                f.Condition.ToString().Contains("i > L") ||
                f.Condition.ToString().Contains("i <= K") ||
                f.Condition.ToString().Contains("i >= L") ||
                f.Condition.ToString().Contains("i < k") ||
                f.Condition.ToString().Contains("i > l") ||
                f.Condition.ToString().Contains("i <= k") ||
                f.Condition.ToString().Contains("i >= l"));

            // Или ищем if с условиями исключения K..L внутри цикла.
            var ifStatements = root.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Where(i =>
                i.Condition.ToString().Contains("i < K") ||
                i.Condition.ToString().Contains("i > L") ||
                i.Condition.ToString().Contains("i <= K") ||
                i.Condition.ToString().Contains("i >= L") ||
                i.Condition.ToString().Contains("i < k") ||
                i.Condition.ToString().Contains("i > l") ||
                i.Condition.ToString().Contains("i <= k") ||
                i.Condition.ToString().Contains("i >= l"))
                .Where(i =>
                i.Ancestors().OfType<ForStatementSyntax>().Any() ||
                i.Ancestors().OfType<WhileStatementSyntax>().Any());

            // Проверяем наличие переменной sum и операций += с элементами массива.
            var sumVariables = root.DescendantNodes()
                .OfType<VariableDeclarationSyntax>()
                .Any(v => v.Variables.Any(v =>
                v.Identifier.Text.ToLower().Contains("sum")));

            var sumOperations = root.DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .Any(a =>
                a.Left.ToString().ToLower().Contains("sum") &&
                a.OperatorToken.ValueText == "+=" &&
                a.Right.ToString().Contains("["));

            return (loops.Any() || ifStatements.Any()) && sumVariables && sumOperations;
        }
    }
}