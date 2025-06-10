using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task20Analyzer
    {
        public static string Analyze(SyntaxNode root)
        {
            MatrixAnalyzer.Analyze(root);

            bool found = root.DescendantNodes()
                .OfType<ForStatementSyntax>()
                .Any(f =>
                {
                    var code = f.ToString().ToLowerInvariant();
                    return code.Contains("[i,") &&
                           code.Contains("sum") &&
                           code.Contains(">") &&
                           (code.Contains("max") || code.Contains("maximum"));
                });

            MatrixAnalyzer.TotalCriteria++;
            if (found)
            {
                MatrixAnalyzer.MetCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("✅ Найдена строка с максимальной суммой элементов");
            }
            else
            {
                MatrixAnalyzer.CriteriaDetails.Add("❌ Не обнаружена логика поиска строки с максимальной суммой");
            }

            return MatrixAnalyzer.PrintResults();
        }
    }
}