using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK01
{
    public static class Task10Analyzer
    {
        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            ArrayAnalyzer.Analyze(root);
            CheckEvenOddOrder(root);
            return ($"{ArrayAnalyzer.MetCriteria}/{ArrayAnalyzer.TotalCriteria+1}", ArrayAnalyzer.PrintResults());
        }

        private static void CheckEvenOddOrder(SyntaxNode root)
        {
            bool hasEvenCheck = root.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any(i => i.Condition.ToString().Contains("%") &&
                         i.Condition.ToString().Contains("== 0"));

            bool hasOddCheck = root.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any(i => i.Condition.ToString().Contains("%") &&
                         (i.Condition.ToString().Contains("!= 0") ||
                          i.Condition.ToString().Contains("> 0") ||
                          i.Condition.ToString().Contains("<> 0")));

            bool hasReverseIndex = root.DescendantNodes()
                .OfType<ForStatementSyntax>()
                .Any(f =>
                {
                    var text = f.ToString();
                    return text.Contains("--") || text.Contains("i--") || text.Contains("i = i - 1");
                });

            if (hasEvenCheck)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Вывод чётных чисел в порядке возрастания индексов выполнен");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Вывод чётных чисел в порядке возрастания индексов не выполнен");
                ArrayAnalyzer.TotalCriteria++;
            }

            if (hasOddCheck && hasReverseIndex)
            {
                ArrayAnalyzer.CriteriaDetails.Add("✅ Вывод нечётных чисел в порядке убывания индексов выполнен");
                ArrayAnalyzer.TotalCriteria++;
                ArrayAnalyzer.MetCriteria++;
            }
            else
            {
                ArrayAnalyzer.CriteriaDetails.Add("❌ Вывод нечётных чисел в порядке убывания индексов не выполнен");
                ArrayAnalyzer.TotalCriteria++;
            }
        }
    }

    public static class ArrayAnalyzer
    {
        public static int TotalCriteria { get; set; } = 4;
        public static int MetCriteria { get; set; } = 0;
        public static List<string> CriteriaDetails { get; } = new List<string>();

        public static string Analyze(SyntaxNode root)
        {
            MetCriteria = 0;
            CriteriaDetails.Clear();

            CheckArrayDimensionInput(root);
            CheckArrayKeyboardInput(root);
            CheckArrayOutput(root);
            CheckInputOutputParameters(root);

            return PrintResults();
        }

        private static void CheckArrayDimensionInput(SyntaxNode root)
        {
            var intVarsWithRead = root.DescendantNodes()
                .OfType<VariableDeclarationSyntax>()
                .Where(v => v.Type.ToString().Contains("int"))
                .SelectMany(v => v.Variables)
                .Where(var => var.Initializer != null &&
                             (var.Initializer.Value.ToString().Contains("ReadLine") ||
                              var.Initializer.Value.ToString().Contains("Parse")))
                .Select(var => var.Identifier.Text)
                .ToList();

            bool usedInArraySize = root.DescendantNodes()
                .OfType<ArrayCreationExpressionSyntax>()
                .Any(arr => intVarsWithRead.Any(varName => arr.ToString().Contains(varName)));

            if (intVarsWithRead.Any() && usedInArraySize)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Ввод размерности массива выполнен");
            }
            else
            {
                CriteriaDetails.Add("❌ Ввод размерности массива не выполнен");
            }
        }

        private static void CheckArrayKeyboardInput(SyntaxNode root)
        {
            var keyboardInputs = root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Where(i => i.Expression.ToString().Contains("ReadLine") ||
                            i.Expression.ToString().Contains("Read"))
                .Where(i => i.Ancestors().OfType<ForStatementSyntax>().Any() ||
                            i.Ancestors().OfType<ForEachStatementSyntax>().Any() ||
                            i.Ancestors().OfType<WhileStatementSyntax>().Any());

            if (keyboardInputs.Any())
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Заполнение массива с клавиатуры выполнено");
            }
            else
            {
                CriteriaDetails.Add("❌ Заполнение массива с клавиатуры не выполнено");
            }
        }

        private static void CheckArrayOutput(SyntaxNode root)
        {
            var arrayOutputs = root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Where(i => i.Expression.ToString().Contains("Write") ||
                            i.Expression.ToString().Contains("WriteLine"))
                .Where(i => i.ArgumentList.Arguments
                    .Any(a => a.ToString().Contains("[") && a.ToString().Contains("]")));

            if (arrayOutputs.Any())
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Вывод элементов массива выполнен");
            }
            else
            {
                CriteriaDetails.Add("❌ Вывод элементов массива не выполнен");
            }
        }

        private static void CheckInputOutputParameters(SyntaxNode root)
        {
            var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            bool hasInputParams = methods.Any(m =>
                m.ParameterList.Parameters.Any(p => p.Type.ToString().Contains("[]")));

            bool hasOutputParams = methods.Any(m =>
                m.ReturnType.ToString().Contains("[]"));

            if (hasInputParams || hasOutputParams)
            {
                MetCriteria++;
                string details = "✅ ";
                if (hasInputParams && hasOutputParams)
                    details += "Входные и выходные параметры массива присутствуют";
                else if (hasInputParams)
                    details += "Только входные параметры массива присутствуют";
                else
                    details += "Только выходные параметры массива присутствуют";

                CriteriaDetails.Add(details);
            }
            else
            {
                CriteriaDetails.Add("❌ Входные и выходные параметры массива отсутствуют");
            }
        }

        public static string PrintResults()
        {
            string result = $"Выполнено критериев: {MetCriteria} из {TotalCriteria}\n";
            result += "Детали:\n";
            foreach (var detail in CriteriaDetails)
            {
                result += $"{detail}\n";
            }
            return result;
        }
    }
}