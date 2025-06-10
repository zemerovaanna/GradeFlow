using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task15Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            ArrayAnalyzer.Analyze(root);
            CheckAveragePositive(root);
            return ($"{ArrayAnalyzer.MetCriteria}/{ArrayAnalyzer.TotalCriteria}", ArrayAnalyzer.PrintResults());
        }

        private static void CheckAveragePositive(SyntaxNode root)
        {
            bool hasPositiveCheck = root.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any(i => i.Condition.ToString().Contains("> 0"));

            bool hasSumAndCount = root.DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Any(v => v.Identifier.Text.ToLower().Contains("sum") || v.Identifier.Text.ToLower().Contains("count"));

            if (hasPositiveCheck && hasSumAndCount)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Вычисление среднего арифметического положительных элементов выполнено");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Вычисление среднего арифметического положительных элементов не выполнено");
                ArrayAnalyzer.TotalCriteria++;
            }
        }
    }
}