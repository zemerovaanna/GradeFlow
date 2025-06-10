using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task26Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MatrixAnalyzer.Analyze(root);

            MatrixAnalyzer.TotalCriteria++;
            bool foundPositiveCount = false;

            var forLoops = root.DescendantNodes().OfType<ForStatementSyntax>().ToList();

            foreach (var outerFor in forLoops)
            {
                var outerText = outerFor.ToString().ToLowerInvariant();
                if (outerText.Contains("for"))
                {
                    var innerFor = outerFor.DescendantNodes().OfType<ForStatementSyntax>().FirstOrDefault();
                    if (innerFor != null)
                    {
                        var innerText = innerFor.ToString().ToLowerInvariant();

                        // Проверяем, есть ли условие для проверки положительности и инкремент счётчика
                        var ifPositiveCheck = root.DescendantNodes()
                            .OfType<IfStatementSyntax>()
                            .Any(ifSt =>
                            {
                                var cond = ifSt.Condition.ToString().ToLowerInvariant();
                                var containsPositive = cond.Contains(">") && (cond.Contains("0") || cond.Contains("zero"));
                                var containsIncrement = ifSt.Statement.ToString().Contains("++") || ifSt.Statement.ToString().Contains("+=1");
                                return containsPositive && containsIncrement;
                            });

                        if (ifPositiveCheck)
                        {
                            foundPositiveCount = true;
                            break;
                        }
                    }
                }
            }

            if (foundPositiveCount)
            {
                MatrixAnalyzer.MetCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("✅ Найден подсчёт положительных элементов в каждой строке");
            }
            else
            {
                MatrixAnalyzer.CriteriaDetails.Add("❌ Не найден подсчёт положительных элементов по строкам");
            }

            return ($"{MatrixAnalyzer.MetCriteria}/{MatrixAnalyzer.TotalCriteria}", MatrixAnalyzer.PrintResults());
        }
    }
}