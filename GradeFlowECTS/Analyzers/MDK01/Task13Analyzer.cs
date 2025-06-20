using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task13Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            ArrayAnalyzer.Analyze(root);
            CheckDivisibleByX(root);
            return ($"{ArrayAnalyzer.MetCriteria}/{ArrayAnalyzer.TotalCriteria+1}", ArrayAnalyzer.PrintResults());
        }

        private static void CheckDivisibleByX(SyntaxNode root)
        {
            bool hasX = root.DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Any(v => v.Identifier.Text.ToLower() == "x");

            bool hasModCheck = root.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any(i => i.Condition.ToString().Contains("%") && i.Condition.ToString().Contains("x"));

            if (hasX && hasModCheck)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Проверка кратности X выполнена");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Проверка кратности X не выполнена");
                ArrayAnalyzer.TotalCriteria++;
            }
        }
    }
}