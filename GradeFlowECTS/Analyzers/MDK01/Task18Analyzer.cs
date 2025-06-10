using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task18Analyzer
    {
        public static string Analyze(SyntaxNode root)
        {
            MatrixAnalyzer.Analyze(root);

            CheckRowSumCalculation(root);

            return MatrixAnalyzer.PrintResults();
        }

        private static void CheckRowSumCalculation(SyntaxNode root)
        {
            var forLoops = root.DescendantNodes().OfType<ForStatementSyntax>().ToList();
            bool foundSum = false;

            foreach (var loop in forLoops)
            {
                if (loop.Statement.ToString().Contains("[K") || loop.Statement.ToString().Contains("[k"))
                {
                    var containsSumLogic = loop.Statement.DescendantNodes()
                        .OfType<AssignmentExpressionSyntax>()
                        .Any(a =>
                            (a.Left.ToString().Contains("sum") || a.Left.ToString().Contains("Sum")) &&
                            (a.OperatorToken.ToString() == "+=" || a.Right.ToString().Contains("["))
                        );

                    if (containsSumLogic)
                    {
                        foundSum = true;
                        break;
                    }
                }
            }

            if (foundSum)
            {
                MatrixAnalyzer.TotalCriteria++;
                MatrixAnalyzer.MetCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("✅ Сумма элементов K-й строки найдена");
            }
            else
            {
                MatrixAnalyzer.TotalCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("❌ Сумма элементов K-й строки не найдена");
            }
        }
    }

}