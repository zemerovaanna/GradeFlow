using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task19Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MatrixAnalyzer.Analyze(root);

            CheckColumnAverageCalculation(root);

            return ($"{MatrixAnalyzer.MetCriteria}/{MatrixAnalyzer.TotalCriteria+1}", MatrixAnalyzer.PrintResults());
        }

        private static void CheckColumnAverageCalculation(SyntaxNode root)
        {
            var forLoops = root.DescendantNodes().OfType<ForStatementSyntax>().ToList();
            bool foundAverage = false;

            foreach (var loop in forLoops)
            {
                if (loop.Statement.ToString().Contains("[") &&
                    (loop.Statement.ToString().Contains(",K") || loop.Statement.ToString().Contains(",k")))
                {
                    var sumLogic = loop.Statement.DescendantNodes()
                        .OfType<AssignmentExpressionSyntax>()
                        .Any(a =>
                            (a.Left.ToString().ToLower().Contains("sum") || a.Left.ToString().ToLower().Contains("total")) &&
                            (a.OperatorToken.ToString() == "+=" || a.Right.ToString().Contains("["))
                        );

                    var division = root.DescendantNodes()
                        .OfType<BinaryExpressionSyntax>()
                        .Any(b => b.OperatorToken.ToString() == "/" &&
                                  (b.Left.ToString().ToLower().Contains("sum") || b.Left.ToString().ToLower().Contains("total")));

                    if (sumLogic && division)
                    {
                        foundAverage = true;
                        break;
                    }
                }
            }

            if (foundAverage)
            {
                MatrixAnalyzer.TotalCriteria++;
                MatrixAnalyzer.MetCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("✅ Среднее арифметическое K-го столбца найдено");
            }
            else
            {
                MatrixAnalyzer.TotalCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("❌ Среднее арифметическое K-го столбца не найдено");
            }
        }
    }
}