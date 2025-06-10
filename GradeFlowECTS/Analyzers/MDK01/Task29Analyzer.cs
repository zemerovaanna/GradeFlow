using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task29Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MatrixAnalyzer.Analyze(root); // базовая проверка: ввод, заполнение, вывод

            MatrixAnalyzer.TotalCriteria++;
            bool foundRowSwap = false;

            // Ищем присваивания между первой и последней строкой (обычно row 0 и row M-1 или N-1)
            var assignments = root.DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .Where(ae => ae.ToString().Contains("[") && ae.ToString().Contains("]"))
                .ToList();

            foreach (var assign in assignments)
            {
                string expr = assign.ToString().ToLowerInvariant();

                bool accessesRow0 = expr.Contains("[0") || expr.Contains("[ 0") || expr.Contains("[0]");
                bool accessesLastRow = expr.Contains("[m-1") || expr.Contains("[n-1") || expr.Contains("[m - 1") || expr.Contains("[n - 1");

                if (accessesRow0 && accessesLastRow)
                {
                    foundRowSwap = true;
                    break;
                }
            }

            if (foundRowSwap)
            {
                MatrixAnalyzer.MetCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("✅ Найден обмен первой и последней строки матрицы");
            }
            else
            {
                MatrixAnalyzer.CriteriaDetails.Add("❌ Не найдено присваивание между первой и последней строками матрицы");
            }

            return ($"{MatrixAnalyzer.MetCriteria}/{MatrixAnalyzer.TotalCriteria}", MatrixAnalyzer.PrintResults());
        }
    }
}