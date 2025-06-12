using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task4
    {
        public static int TotalCriteria { get; set; } = 5;
        public static int MetCriteria { get; set; } = 0;
        public static List<string> CriteriaDetails { get; } = new();

        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MetCriteria = 0;
            CriteriaDetails.Clear();

            CheckMatrixClass(root);
            CheckNonZeroCountingMethod(root);
            CheckMatrixSizeLimit(root);
            CheckConsoleInput(root);
            CheckElementRange(root);

            return ($"{MetCriteria}/{TotalCriteria}", PrintResults());
        }

        private static void CheckMatrixClass(SyntaxNode root)
        {
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach (var cls in classes)
            {
                var name = cls.Identifier.Text.ToLower();
                if (name.Contains("matrix") || name.Contains("матриц"))
                {
                    MetCriteria++;
                    CriteriaDetails.Add("✅ Найден класс, связанный с матрицей");
                    return;
                }
            }

            CriteriaDetails.Add("❌ Класс матрицы не найден");
        }

        private static void CheckNonZeroCountingMethod(SyntaxNode root)
        {
            var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (var method in methods)
            {
                var bodyText = method.Body?.ToString().ToLower() ?? method.ExpressionBody?.ToString().ToLower() ?? "";

                bool countsNonZero = bodyText.Contains("!= 0") || bodyText.Contains("> 0") || bodyText.Contains("< 0");

                if (countsNonZero && (bodyText.Contains("count") || bodyText.Contains("кол") || bodyText.Contains("счет")))
                {
                    MetCriteria++;
                    CriteriaDetails.Add("✅ Найден метод, считающий ненулевые элементы");
                    return;
                }
            }

            CriteriaDetails.Add("❌ Метод подсчёта ненулевых элементов не найден");
        }

        private static void CheckMatrixSizeLimit(SyntaxNode root)
        {
            var text = root.ToString().ToLower();
            if (text.Contains("n < 10") || text.Contains("n<=9") || text.Contains("n>0") || text.Contains("0<n"))
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Присутствует проверка диапазона для n (0 < n < 10)");
            }
            else
            {
                CriteriaDetails.Add("❌ Проверка диапазона размера матрицы (0 < n < 10) не найдена");
            }
        }

        private static void CheckConsoleInput(SyntaxNode root)
        {
            var text = root.ToString().ToLower();
            if (text.Contains("console.readline"))
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Ввод элементов с клавиатуры реализован через Console.ReadLine()");
            }
            else
            {
                CriteriaDetails.Add("❌ Не найден ввод с клавиатуры через Console.ReadLine()");
            }
        }

        private static void CheckElementRange(SyntaxNode root)
        {
            var text = root.ToString().ToLower();

            bool checksMin = text.Contains(">= -10") || text.Contains("> -11") || text.Contains(">-11");
            bool checksMax = text.Contains("<= 20") || text.Contains("< 21") || text.Contains("<21");

            if (checksMin && checksMax)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Есть проверка, что элементы в диапазоне [-10; 20]");
            }
            else
            {
                CriteriaDetails.Add("❌ Не найдена проверка диапазона [-10; 20] для элементов матрицы");
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