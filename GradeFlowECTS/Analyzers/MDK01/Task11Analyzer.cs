using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task11Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            ArrayAnalyzer.Analyze(root);
            CheckMinMax(root);
            return ($"{ArrayAnalyzer.MetCriteria}/{ArrayAnalyzer.TotalCriteria}", ArrayAnalyzer.PrintResults());
        }

        private static void CheckMinMax(SyntaxNode root)
        {
            bool hasMin = root.DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Any(v => v.Identifier.Text.ToLower().Contains("min"));

            bool hasMax = root.DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Any(v => v.Identifier.Text.ToLower().Contains("max"));

            bool hasIndex = root.DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Any(v => v.Identifier.Text.ToLower().Contains("index"));

            if (hasMin && hasMax && hasIndex)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Нахождение минимума и максимума с индексами выполнено");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Нахождение минимума и максимума с индексами не выполнено");
                ArrayAnalyzer.TotalCriteria++;
            }
        }
    }
}