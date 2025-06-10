using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task25Analyzer
    {
        public static string Analyze(SyntaxNode root)
        {
            MatrixAnalyzer.Analyze(root);

            MatrixAnalyzer.TotalCriteria++;
            bool foundMaxSum = false;
            bool foundMinSum = false;

            // Поиск переменных/вычислений max и min суммы
            var sumVars = root.DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Where(v => v.Identifier.Text.ToLowerInvariant().Contains("sum"))
                .Select(v => v.Identifier.Text.ToLowerInvariant());

            var maxSumFound = root.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any(ifSt =>
                {
                    var cond = ifSt.Condition.ToString().ToLowerInvariant();
                    return cond.Contains("max") && cond.Contains("sum");
                });

            var minSumFound = root.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any(ifSt =>
                {
                    var cond = ifSt.Condition.ToString().ToLowerInvariant();
                    return cond.Contains("min") && cond.Contains("sum");
                });

            if (maxSumFound) foundMaxSum = true;
            if (minSumFound) foundMinSum = true;

            if (foundMaxSum && foundMinSum)
            {
                MatrixAnalyzer.MetCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("✅ Найдены строки с максимальной и минимальной суммой");
            }
            else
            {
                if (!foundMaxSum)
                    MatrixAnalyzer.CriteriaDetails.Add("❌ Логика поиска строки с максимальной суммой не найдена");
                if (!foundMinSum)
                    MatrixAnalyzer.CriteriaDetails.Add("❌ Логика поиска строки с минимальной суммой не найдена");
            }

            return MatrixAnalyzer.PrintResults();
        }
    }
}