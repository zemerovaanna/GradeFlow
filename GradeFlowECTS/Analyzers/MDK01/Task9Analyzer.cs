using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task9Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MatrixAnalyzer.Analyze(root);
            CheckColumnOutput(root);
            return ($"{MatrixAnalyzer.MetCriteria}/{MatrixAnalyzer.TotalCriteria+1}", MatrixAnalyzer.PrintResults());
        }

        private static void CheckColumnOutput(SyntaxNode root)
        {
            var colAccesses = root.DescendantNodes()
                .OfType<ElementAccessExpressionSyntax>()
                .Where(e => e.ArgumentList.Arguments.Count == 2)
                .Where(e =>
                {
                    var args = e.ArgumentList.Arguments;
                    return args[1].ToString().Contains("K") || args[1].ToString().Contains("K - 1");
                });

            if (colAccesses.Any())
            {
                MatrixAnalyzer.MetCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("✅ Вывод K-го столбца матрицы выполнен");
                MatrixAnalyzer.TotalCriteria++;
            }
            else
            {
                MatrixAnalyzer.CriteriaDetails.Add("❌ Вывод K-го столбца матрицы не выполнен");
                MatrixAnalyzer.TotalCriteria++;
            }
        }
    }

   /* public static class Task9Analyzer
    {
        public static string Analyze(SyntaxNode root)
        {
            MatrixAnalyzer.Analyze(root); // Общая проверка
            CheckColumnOutput(root);      // Специфическая проверка
            return MatrixAnalyzer.PrintResults();
        }

        private static void CheckColumnOutput(SyntaxNode root)
        {
            var colAccesses = root.DescendantNodes()
                .OfType<ElementAccessExpressionSyntax>()
                .Where(e => e.ArgumentList.Arguments.Count == 2)
                .Where(e =>
                {
                    var args = e.ArgumentList.Arguments;
                    return args[1].ToString().Contains("K") || args[1].ToString().Contains("K - 1");
                });

            if (colAccesses.Any())
            {
                MatrixAnalyzer.MetCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("✅ Вывод K-го столбца матрицы выполнен");
                MatrixAnalyzer.TotalCriteria++;
            }
            else
            {
                MatrixAnalyzer.CriteriaDetails.Add("❌ Вывод K-го столбца матрицы не выполнен");
                MatrixAnalyzer.TotalCriteria++;
            }
        }
    }*/
}