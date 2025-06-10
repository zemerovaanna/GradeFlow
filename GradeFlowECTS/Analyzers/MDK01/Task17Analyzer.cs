using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task17Analyzer
    {
        public static string Analyze(SyntaxNode root)
        {
            ArrayAnalyzer.Analyze(root);
            CheckTwoMinElements(root);
            return ArrayAnalyzer.PrintResults();
        }

        private static void CheckTwoMinElements(SyntaxNode root)
        {
            // Поиск переменных с именами, содержащими "min"
            var minVars = root.DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Where(v => v.Identifier.Text.ToLower().Contains("min"))
                .ToList();

            // Поиск переменных с именами, содержащими "index"
            var indexVars = root.DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Where(v => v.Identifier.Text.ToLower().Contains("index"))
                .ToList();

            bool hasTwoMins = minVars.Count >= 2;
            bool hasTwoIndices = indexVars.Count >= 2;

            if (hasTwoMins && hasTwoIndices)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Найдены два наименьших элемента и их индексы");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                if (!hasTwoMins)
                    ArrayAnalyzer.CriteriaDetails.Add("❌ Не найдены две переменные для минимальных значений");

                if (!hasTwoIndices)
                    ArrayAnalyzer.CriteriaDetails.Add("❌ Не найдены две переменные для индексов минимальных значений");

                ArrayAnalyzer.TotalCriteria++;
            }
        }
    }
}