using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers
{
    public static class MatrixAnalyzer
    {
        public static int TotalCriteria { get; set; } = 3;
        public static int MetCriteria { get; set; } = 0;
        public static List<string> CriteriaDetails { get; } = new List<string>();

        public static string Analyze(SyntaxNode root)
        {
            MetCriteria = 0;
            CriteriaDetails.Clear();

            CheckMatrixDimensionInput(root);
            CheckMatrixKeyboardInput(root);
            CheckMatrixOutput(root);

            return PrintResults();
        }

        private static void CheckMatrixDimensionInput(SyntaxNode root)
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

            bool usedInMatrixSize = root.DescendantNodes()
                .OfType<ArrayCreationExpressionSyntax>()
                .Any(arr => arr.Type.RankSpecifiers.Any(r => r.Rank == 2) &&
                            intVarsWithRead.Any(varName => arr.ToString().Contains(varName)));

            if (intVarsWithRead.Any() && usedInMatrixSize)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Ввод размеров матрицы выполнен");
            }
            else
            {
                CriteriaDetails.Add("❌ Ввод размеров матрицы не выполнен");
            }
        }

        private static void CheckMatrixKeyboardInput(SyntaxNode root)
        {
            var keyboardInputs = root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Where(i => i.Expression.ToString().Contains("ReadLine") || i.Expression.ToString().Contains("Read"))
                .Where(i => i.Ancestors().OfType<ForStatementSyntax>().Count() >= 2); // Два вложенных цикла для ввода матрицы

            if (keyboardInputs.Any())
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Заполнение матрицы с клавиатуры выполнено");
            }
            else
            {
                CriteriaDetails.Add("❌ Заполнение матрицы с клавиатуры не выполнено");
            }
        }

        private static void CheckMatrixOutput(SyntaxNode root)
        {
            var matrixOutputs = root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Where(i => i.Expression.ToString().Contains("Write") || i.Expression.ToString().Contains("WriteLine"))
                .Where(i => i.ArgumentList.Arguments
                    .Any(a => a.ToString().Contains("[") && a.ToString().Contains(","))); // Доступ к двумерному массиву

            if (matrixOutputs.Any())
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Вывод элементов матрицы выполнен");
            }
            else
            {
                CriteriaDetails.Add("❌ Вывод элементов матрицы не выполнен");
            }
        }

        public static string PrintResults()
        {
            string result = $"Выполнено критериев: {MetCriteria} из {TotalCriteria}\nДетали:\n";
            foreach (var detail in CriteriaDetails)
            {
                result += $"{detail}\n";
            }
            return result;
        }
    }
}