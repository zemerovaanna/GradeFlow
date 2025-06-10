using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task24Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MatrixAnalyzer.Analyze(root);

            MatrixAnalyzer.TotalCriteria++;
            bool foundMaxInColumns = false;

            // Проверяем цикл по столбцам (N), поиск максимума, хранение индекса
            var forLoops = root.DescendantNodes().OfType<ForStatementSyntax>().ToList();

            foreach (var outerFor in forLoops)
            {
                var outerText = outerFor.ToString().ToLowerInvariant();
                // Поиск цикла по столбцам (часто цикл по j)
                if (outerText.Contains("for") && (outerText.Contains("j") || outerText.Contains("col")))
                {
                    // Вложенный цикл по строкам (i)
                    var innerFor = outerFor.DescendantNodes().OfType<ForStatementSyntax>().FirstOrDefault();
                    if (innerFor != null)
                    {
                        var innerText = innerFor.ToString().ToLowerInvariant();
                        // Ищем сравнение для max и сохранение индекса строки
                        var containsMaxPattern = root.DescendantNodes()
                            .OfType<IfStatementSyntax>()
                            .Any(ifSt =>
                            {
                                var cond = ifSt.Condition.ToString().ToLowerInvariant();
                                return cond.Contains(">") && (cond.Contains("max") || cond.Contains("maximum"));
                            });

                        var hasIndexStore = root.DescendantNodes()
                            .OfType<AssignmentExpressionSyntax>()
                            .Any(ae =>
                            {
                                var txt = ae.ToString().ToLowerInvariant();
                                return txt.Contains("index") || txt.Contains("rowindex") || txt.Contains("maxindex");
                            });

                        if (containsMaxPattern && hasIndexStore)
                        {
                            foundMaxInColumns = true;
                            break;
                        }
                    }
                }
            }

            if (foundMaxInColumns)
            {
                MatrixAnalyzer.MetCriteria++;
                MatrixAnalyzer.CriteriaDetails.Add("✅ Максимальный элемент и индекс строки по каждому столбцу найдены");
            }
            else
            {
                MatrixAnalyzer.CriteriaDetails.Add("❌ Не найдена логика поиска максимального элемента и индекса в столбцах");
            }

            return ($"{MatrixAnalyzer.MetCriteria}/{MatrixAnalyzer.TotalCriteria}", MatrixAnalyzer.PrintResults());
        }
    }
}