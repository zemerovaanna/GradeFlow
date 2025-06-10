using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task14Analyzer
    {
        public static string Analyze(SyntaxNode root)
        {
            ArrayAnalyzer.Analyze(root);
            CheckReplaceEvenWithZero(root);
            return ArrayAnalyzer.PrintResults();
        }

        private static void CheckReplaceEvenWithZero(SyntaxNode root)
        {
            bool hasEvenCheck = root.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any(i => i.Condition.ToString().Contains("% 2 == 0"));

            bool hasAssignmentZero = root.DescendantNodes()
                .OfType<ExpressionStatementSyntax>()
                .Any(e => e.ToString().Contains("= 0"));

            if (hasEvenCheck && hasAssignmentZero)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Замена чётных элементов на нули выполнена");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Замена чётных элементов на нули не выполнена");
                ArrayAnalyzer.TotalCriteria++;
            }
        }
    }
}