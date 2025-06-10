using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task1Analyzer
    {
        public static string Analyze(SyntaxNode root)
        {
            ArrayAnalyzer.Analyze(root);
            CheckSpecificConditions(root);
            return ArrayAnalyzer.PrintResults();
        }

        private static void CheckSpecificConditions(SyntaxNode root)
        {
            bool hasOddOutput = root.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any(i => i.Condition.ToString().Contains("% 2") ||
                         i.Condition.ToString().Contains("mod") ||
                         i.Condition.ToString().Contains("нечетн"));

            bool hasCount = root.DescendantNodes()
                .OfType<VariableDeclarationSyntax>()
                .Any(v => v.Variables.Any(v =>
                    v.Identifier.Text == "k" ||
                    v.Identifier.Text == "K"));

            if (hasOddOutput)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Вывод нечетных чисел выполнен");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Вывод нечетных чисел не выполнен");
                ArrayAnalyzer.TotalCriteria++;
            }

            if (hasCount)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Вывод количества выполнен");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Вывод количества не выполнен");
                ArrayAnalyzer.TotalCriteria++;
            }
        }
    }
}