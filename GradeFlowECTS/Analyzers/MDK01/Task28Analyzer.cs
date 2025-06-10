using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task28Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MatrixAnalyzer.Analyze(root);

            MatrixAnalyzer.TotalCriteria++;
            bool foundMaxInColumn = false;

            var forLoops = root.DescendantNodes().OfType<ForStatementSyntax>().ToList();

            foreach (var outerFor in forLoops)
            {
                var outerText = outerFor.ToString().ToLowerInvariant();
                // цикл по столбцам
                if (outerText.Contains("for"))
                {
                    var innerFor = outerFor.DescendantNodes().OfType<ForStatementSyntax>().FirstOrDefault();
                    if (innerFor != null)
                    {
                        var innerText = innerFor.ToString().ToLowerInvariant();

                        var ifMaxPattern = root.DescendantNodes()
                            .OfType<IfStatementSyntax>()
                            .Any(ifSt =>
                            {
                                var cond = ifSt.Condition.ToString().ToLowerInvariant();
                                return cond.Contains(">") && (cond.Contains("max") || cond.Contains("maximum"));
                            });

                        var hasIndexSave = root.DescendantNodes()
                            .OfType<AssignmentExpressionSyntax>()
                            .Any(ae =>
                            {
                                var txt = ae.ToString().ToLowerInvariant();
                                return txt.Contains("index") || txt.Contains("rowindex");
                            });

                        if (ifMaxPattern && hasIndexSave)
                        {
                            foundMaxInColumn = true;
                            break;
                        }
                    }
                }
            }

            if (foundMaxInColumn)
            {
                MatrixAnalyzer.MetCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("✅ Найден максимальный элемент в столбцах с индексами");
            }
            else
            {
                MatrixAnalyzer.CriteriaDetails.Add("❌ Не найдена логика поиска максимального элемента в столбцах");
            }

            return ($"{MatrixAnalyzer.MetCriteria}/{MatrixAnalyzer.TotalCriteria}", MatrixAnalyzer.PrintResults());
        }
    }
}