using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task13
    {
        public static int TotalCriteria => 2;
        public static int MetCriteria { get; private set; }
        public static List<string> Details { get; } = new();

        public static string Analyze(SyntaxNode root)
        {
            MetCriteria = 0;
            Details.Clear();

            var hasMatrixInput = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Any(m => m.ToString().ToLower().Contains("console.read") &&
                          (m.ToString().ToLower().Contains("[i][j]") || m.ToString().ToLower().Contains("matrix")));

            if (hasMatrixInput)
            {
                MetCriteria++;
                Details.Add("✅ Найден ввод матрицы с клавиатуры");
            }
            else
            {
                Details.Add("❌ Ввод матрицы не найден");
            }

            var hasMinLogic = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Any(m => m.ToString().ToLower().Contains("min") || m.ToString().Contains("<"));

            if (hasMinLogic)
            {
                MetCriteria++;
                Details.Add("✅ Найдено вычисление минимума в матрице");
            }
            else
            {
                Details.Add("❌ Метод нахождения минимума не найден");
            }

            return Print();
        }

        private static string Print()
        {
            return $"Задание 13: выполнено {MetCriteria} из {TotalCriteria}\n" + string.Join("\n", Details);
        }
    }
}