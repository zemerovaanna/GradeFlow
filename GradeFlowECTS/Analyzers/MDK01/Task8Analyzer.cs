using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task8Analyzer
    {
        public static string Analyze(SyntaxNode root)
        {
            MatrixAnalyzer.Analyze(root);
            CheckRowOutput(root);
            return MatrixAnalyzer.PrintResults();
        }

        private static void CheckRowOutput(SyntaxNode root)
        {
            var rowAccesses = root.DescendantNodes()
                .OfType<ElementAccessExpressionSyntax>()
                .Where(e => e.ArgumentList.Arguments.Count == 2)
                .Where(e =>
                {
                    var args = e.ArgumentList.Arguments;
                    return args[0].ToString().Contains("K") || args[0].ToString().Contains("K - 1");
                });

            if (rowAccesses.Any())
            {
                MatrixAnalyzer.MetCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("✅ Вывод K-й строки матрицы выполнен");
                MatrixAnalyzer.TotalCriteria++;
            }
            else
            {
                MatrixAnalyzer.CriteriaDetails.Add("❌ Вывод K-й строки матрицы не выполнен");
                MatrixAnalyzer.TotalCriteria++;
            }
        }
    }

    /*    public static class Task8Analyzer
        {
            public static string Analyze(SyntaxNode root)
            {
                MatrixAnalyzer.Analyze(root);
                CheckRowOutput(root);
                return MatrixAnalyzer.PrintResults();
            }

            private static void CheckRowOutput(SyntaxNode root)
            {
                // Ищем доступ к элементам строки: matrix[K - 1, j] или аналог
                var rowAccess = root.DescendantNodes()
                    .OfType<ElementAccessExpressionSyntax>()
                    .Where(e => e.ArgumentList.Arguments.Count == 2)
                    .Where(e =>
                    {
                        var args = e.ArgumentList.Arguments;
                        return args[0].ToString().Contains("K") || args[0].ToString().Contains("k");
                    });

                if (rowAccess.Any())
                {
                    MatrixAnalyzer.CriteriaDetails.Add("✅ Вывод элементов K-й строки выполнен");
                    MatrixAnalyzer.TotalCriteria++;
                    MatrixAnalyzer.MetCriteria++;
                }
                else
                {
                    MatrixAnalyzer.CriteriaDetails.Add("❌ Вывод элементов K-й строки не выполнен");
                    MatrixAnalyzer.TotalCriteria++;
                }
            }
        }*/
}