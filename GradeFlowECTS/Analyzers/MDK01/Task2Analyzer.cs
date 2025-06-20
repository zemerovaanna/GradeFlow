using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task2Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            ArrayAnalyzer.Analyze(root);
            CheckSpecificConditions(root);
            return ($"{ArrayAnalyzer.MetCriteria}/{ArrayAnalyzer.TotalCriteria+1}", ArrayAnalyzer.PrintResults());
        }

        private static void CheckSpecificConditions(SyntaxNode root)
        {
            // Проверка вывода четных чисел в порядке возрастания индексов.
            bool hasEvenAscendingOutput = CheckEvenNumbersOutput(root, true);

            // Проверка вывода нечетных чисел в порядке убывания индексов.
            bool hasOddDescendingOutput = CheckOddNumbersOutput(root, false);

            // Добавляем критерии проверки.
            if (hasEvenAscendingOutput)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Вывод четных чисел в порядке возрастания индексов выполнен");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Вывод четных чисел в порядке возрастания индексов не выполнен");
                ArrayAnalyzer.TotalCriteria++;
            }

            if (hasOddDescendingOutput)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Вывод нечетных чисел в порядке убывания индексов выполнен");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Вывод нечетных чисел в порядке убывания индексов не выполнен");
                ArrayAnalyzer.TotalCriteria++;
            }
        }

        private static bool CheckEvenNumbersOutput(SyntaxNode root, bool ascending)
        {
            // Проверка условий для четных чисел.
            var evenChecks = root.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Where(i =>
                i.Condition.ToString().Contains("% 2 == 0") ||
                i.Condition.ToString().Contains("mod 2 == 0") ||
                i.Condition.ToString().Contains("четн"))
                .SelectMany(i =>
                i.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Where(inv =>
                inv.Expression.ToString().Contains("Write"))
                .ToList());

            // Проверка порядка вывода (по возрастанию индексов).
            var forStatements = root.DescendantNodes()
                .OfType<ForStatementSyntax>()
                .Where(f =>
                f.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any(i =>
                i.Condition.ToString().Contains("% 2 == 0")))
                .Where(f =>
                f.Condition.ToString().Contains("<") &&
                f.Incrementors.Any(inc => inc.ToString().Contains("++")))
                .ToList();

            return evenChecks.Any() || forStatements.Any();
        }

        private static bool CheckOddNumbersOutput(SyntaxNode root, bool ascending)
        {
            // Проверка условий для нечетных чисел.
            var oddChecks = root.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Where(i =>
                i.Condition.ToString().Contains("% 2 != 0") ||
                i.Condition.ToString().Contains("mod 2 != 0") ||
                i.Condition.ToString().Contains("нечетн"))
                .SelectMany(i =>
                i.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Where(inv => inv.Expression.ToString().Contains("Write")))
                .ToList();

            // Проверка порядка вывода (по убыванию индексов).
            var forStatements = root.DescendantNodes()
                .OfType<ForStatementSyntax>()
                .Where(f =>
                f.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any(i =>
                i.Condition.ToString().Contains("% 2 != 0")))
                .Where(f =>
                f.Condition.ToString().Contains(">=") &&
                f.Initializers.Any(init => init.ToString().Contains("=")) &&
                f.Incrementors.Any(inc => inc.ToString().Contains("--")))
                .ToList();

            return oddChecks.Any() || forStatements.Any();
        }
    }
}