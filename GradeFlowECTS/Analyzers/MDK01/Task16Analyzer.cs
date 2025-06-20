using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task16Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            ArrayAnalyzer.Analyze(root);
            CheckExcludeMultiplesOfThree(root);
            return ($"{ArrayAnalyzer.MetCriteria}/{ArrayAnalyzer.TotalCriteria+1}", ArrayAnalyzer.PrintResults());
        }

        private static void CheckExcludeMultiplesOfThree(SyntaxNode root)
        {
            bool hasMod3Check = root.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any(i => i.Condition.ToString().Contains("% 3"));

            bool hasNewArray = root.DescendantNodes()
                .OfType<VariableDeclarationSyntax>()
                .Any(v => v.Type.ToString().Contains("[]"));

            if (hasMod3Check && hasNewArray)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Исключение элементов, кратных 3, выполнено");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Исключение элементов, кратных 3, не выполнено");
                ArrayAnalyzer.TotalCriteria++;
            }
        }
    }
}