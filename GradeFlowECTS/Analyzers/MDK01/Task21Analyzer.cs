using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task21Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MatrixAnalyzer.Analyze(root);

            var arrays2D = root.DescendantNodes()
                .OfType<ArrayCreationExpressionSyntax>()
                .Where(a => a.Type.RankSpecifiers.Any(r => r.Rank == 2))
                .ToList();

            bool hasTwo2DArrays = arrays2D.Count >= 2;

            bool hasTransposeAccess = root.DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .Any(a =>
                {
                    var text = a.ToString().ToLowerInvariant();
                    return text.Contains("[i,j]") && text.Contains("[j,i]");
                });

            MatrixAnalyzer.TotalCriteria++;
            if (hasTwo2DArrays && hasTransposeAccess)
            {
                MatrixAnalyzer.MetCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("✅ Транспонирование матрицы выполнено");
            }
            else
            {
                MatrixAnalyzer.CriteriaDetails.Add("❌ Транспонирование матрицы не выполнено");
            }

            return ($"{MatrixAnalyzer.MetCriteria}/{MatrixAnalyzer.TotalCriteria}", MatrixAnalyzer.PrintResults());
        }
    }
}