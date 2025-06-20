using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task4Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            ArrayAnalyzer.Analyze(root);
            CheckSpecificConditions(root);
            return ($"{ArrayAnalyzer.MetCriteria}/{ArrayAnalyzer.TotalCriteria+1}", ArrayAnalyzer.PrintResults());
        }

        private static void CheckSpecificConditions(SyntaxNode root)
        {
            // Проверка ввода K и L.
            bool hasKLInput = CheckKLInput(root);

            // Проверка вычисления среднего арифметического.
            bool hasAverageCalculation = CheckAverageCalculation(root);

            // Проверка исключения элементов от K до L.
            bool hasExclusionCheck = CheckExclusionCondition(root);

            // Обновляем общие критерии.
            UpdateCriteria(hasKLInput, hasAverageCalculation, hasExclusionCheck);
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

        private static bool CheckAverageCalculation(SyntaxNode root)
        {
            // Проверяем наличие вычисления суммы.
            bool hasSum = root.DescendantNodes()
                .OfType<VariableDeclarationSyntax>()
                .Any(v => v.Variables.Any(v =>
                    v.Initializer?.Value.ToString().Contains("+=") == true ||
                    v.Initializer?.Value.ToString().Contains("sum") == true));

            // Проверяем наличие вычисления количества элементов.
            bool hasCount = root.DescendantNodes()
                .OfType<VariableDeclarationSyntax>()
                .Any(v => v.Variables.Any(v =>
                    v.Identifier.Text.ToLower().Contains("count")));

            // Проверяем наличие деления суммы на количество.
            bool hasDivision = root.DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .Any(b => b.OperatorToken.Text == "/" &&
                         (b.Left.ToString().Contains("sum") ||
                          b.Right.ToString().Contains("count")));

            return hasSum && hasCount && hasDivision;
        }

        private static bool CheckExclusionCondition(SyntaxNode root)
        {
            // Проверяем наличие условия для исключения элементов от K до L.
            bool hasExclusion = root.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any(i =>
                i.Condition.ToString().Contains("i < K") ||
                i.Condition.ToString().Contains("i < k") ||
                i.Condition.ToString().Contains("i > L") ||
                i.Condition.ToString().Contains("i > l") ||
                i.Condition.ToString().Contains("!(i >= K && i <= L)") ||
                i.Condition.ToString().Contains("!(i >= k && i <= l)"));

            // Или проверяем цикл с пропуском элементов от K до L.
            bool hasLoopWithSkip = root.DescendantNodes()
                .OfType<ForStatementSyntax>()
                .Any(f => f.Statement.DescendantNodes()
                    .OfType<IfStatementSyntax>()
                    .Any(i => i.Condition.ToString().Contains("i < K") ||
                    i.Condition.ToString().Contains("i < k") ||
                    i.Condition.ToString().Contains("i > L") ||
                    i.Condition.ToString().Contains("i > l")));

            return hasExclusion || hasLoopWithSkip;
        }

        private static void UpdateCriteria(bool hasKLInput, bool hasAverageCalculation, bool hasExclusionCheck)
        {
            ArrayAnalyzer.TotalCriteria += 3;

            if (hasKLInput)
            {
                ArrayAnalyzer.MetCriteria++;
                ArrayAnalyzer.CriteriaDetails.Add("✅ Ввод K и L выполнен");
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Ввод K и L не выполнен");
            }

            if (hasAverageCalculation)
            {
                ArrayAnalyzer.MetCriteria++;
                ArrayAnalyzer.CriteriaDetails.Add("✅ Вычисление среднего арифметического выполнено");
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Вычисление среднего арифметического не выполнено");
            }

            if (hasExclusionCheck)
            {
                ArrayAnalyzer.MetCriteria++;
                ArrayAnalyzer.CriteriaDetails.Add("✅ Исключение элементов от K до L выполнено");
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Исключение элементов от K до L не выполнено");
            }
        }
    }
}