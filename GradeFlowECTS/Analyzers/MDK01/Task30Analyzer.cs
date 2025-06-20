using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task30Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MatrixAnalyzer.Analyze(root); // проверка ввода, заполнения и вывода

            MatrixAnalyzer.TotalCriteria++;
            bool foundColumnSwap = false;

            // Ищем присваивания между первым и последним столбцом (обычно col 0 и col M-1 или N-1)
            var assignments = root.DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .Where(ae => ae.ToString().Contains("[") && ae.ToString().Contains("]"))
                .ToList();

            foreach (var assign in assignments)
            {
                string expr = assign.ToString().ToLowerInvariant();

                bool accessesCol0 = expr.Contains(",0") || expr.Contains(", 0") || expr.Contains("[0]");
                bool accessesLastCol = expr.Contains(",m-1") || expr.Contains(",n-1") || expr.Contains(", m - 1") || expr.Contains(", n - 1");

                if (accessesCol0 && accessesLastCol)
                {
                    foundColumnSwap = true;
                    break;
                }
            }

            if (foundColumnSwap)
            {
                MatrixAnalyzer.MetCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("✅ Найден обмен первого и последнего столбца матрицы");
            }
            else
            {
                MatrixAnalyzer.CriteriaDetails.Add("❌ Не найдено присваивание между первым и последним столбцами матрицы");
            }

            return ($"{MatrixAnalyzer.MetCriteria}/{MatrixAnalyzer.TotalCriteria+1}", MatrixAnalyzer.PrintResults());
        }
    }
}