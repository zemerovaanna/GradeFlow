using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task23Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MatrixAnalyzer.Analyze(root);

            var forLoops = root.DescendantNodes().OfType<ForStatementSyntax>().ToList();

            bool hasColumnSwap = forLoops.Any(f =>
            {
                var code = f.ToString().ToLowerInvariant();
                return (code.Contains("[,k") || code.Contains("[,l") ||
                        code.Contains("[i,k") || code.Contains("[i,l")) &&
                       code.Contains("temp") &&
                       f.DescendantNodes().OfType<AssignmentExpressionSyntax>().Count() >= 3;
            });

            MatrixAnalyzer.TotalCriteria++;
            if (hasColumnSwap)
            {
                MatrixAnalyzer.MetCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("✅ Обнаружена замена столбцов K и L");
            }
            else
            {
                MatrixAnalyzer.CriteriaDetails.Add("❌ Замена столбцов K и L не найдена");
            }

            return ($"{MatrixAnalyzer.MetCriteria}/{MatrixAnalyzer.TotalCriteria+1}", MatrixAnalyzer.PrintResults());
        }
    }
}