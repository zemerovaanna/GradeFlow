using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task12Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            ArrayAnalyzer.Analyze(root);
            CheckSumRange(root);
            return ($"{ArrayAnalyzer.MetCriteria}/{ArrayAnalyzer.TotalCriteria}", ArrayAnalyzer.PrintResults());
        }

        private static void CheckSumRange(SyntaxNode root)
        {
            bool hasKandL = root.DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Any(v => v.Identifier.Text.ToLower() == "k" || v.Identifier.Text.ToLower() == "l");

            bool hasSumLoop = root.DescendantNodes()
                .OfType<ForStatementSyntax>()
                .Any(f => f.ToString().ToLower().Contains("sum"));

            if (hasKandL && hasSumLoop)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Суммирование элементов от K до L выполнено");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Суммирование элементов от K до L не выполнено");
                ArrayAnalyzer.TotalCriteria++;
            }
        }
    }
}