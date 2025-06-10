using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task27Analyzer
    {
        public static string Analyze(SyntaxNode root)
        {
            MatrixAnalyzer.Analyze(root);

            MatrixAnalyzer.TotalCriteria++;
            bool foundMinInRow = false;

            var forLoops = root.DescendantNodes().OfType<ForStatementSyntax>().ToList();

            foreach (var outerFor in forLoops)
            {
                var outerText = outerFor.ToString().ToLowerInvariant();
                // цикл по строкам
                if (outerText.Contains("for"))
                {
                    var innerFor = outerFor.DescendantNodes().OfType<ForStatementSyntax>().FirstOrDefault();
                    if (innerFor != null)
                    {
                        var innerText = innerFor.ToString().ToLowerInvariant();
                        // логика поиска минимума с сохранением индексов
                        var ifMinPattern = root.DescendantNodes()
                            .OfType<IfStatementSyntax>()
                            .Any(ifSt =>
                            {
                                var cond = ifSt.Condition.ToString().ToLowerInvariant();
                                return cond.Contains("<") && (cond.Contains("min") || cond.Contains("minimum"));
                            });

                        var hasIndexSave = root.DescendantNodes()
                            .OfType<AssignmentExpressionSyntax>()
                            .Any(ae =>
                            {
                                var txt = ae.ToString().ToLowerInvariant();
                                return txt.Contains("index") || txt.Contains("rowindex");
                            });

                        if (ifMinPattern && hasIndexSave)
                        {
                            foundMinInRow = true;
                            break;
                        }
                    }
                }
            }

            if (foundMinInRow)
            {
                MatrixAnalyzer.MetCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("✅ Найден минимальный элемент в строках с индексами");
            }
            else
            {
                MatrixAnalyzer.CriteriaDetails.Add("❌ Не найдена логика поиска минимального элемента в строках");
            }

            return MatrixAnalyzer.PrintResults();
        }
    }
}