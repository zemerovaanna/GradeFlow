using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task6
    {
        public static int TotalCriteria { get; set; } = 5;
        public static int MetCriteria { get; set; } = 0;
        public static List<string> CriteriaDetails { get; } = new();

        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MetCriteria = 0;
            CriteriaDetails.Clear();

            CheckPrivateFields(root);
            CheckReadFromFileMethod(root);
            CheckConsoleOutputMethod(root);
            CheckIsAllNumbersMethod(root);
            CheckSumIfAllNumbersMethod(root);

            return ($"{MetCriteria}/{TotalCriteria}", PrintResults());
        }

        private static void CheckPrivateFields(SyntaxNode root)
        {
            var classNodes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach (var cls in classNodes)
            {
                var fields = cls.Members.OfType<FieldDeclarationSyntax>()
                    .Where(f => f.Modifiers.Any(m => m.Text == "private")).ToList();

                bool hasFileName = fields.Any(f =>
                    f.Declaration.Type.ToString().ToLower().Contains("string") &&
                    f.ToString().ToLower().Contains("file"));

                bool hasArrayData = fields.Any(f =>
                    (f.Declaration.Type.ToString().ToLower().Contains("[]") ||
                     f.Declaration.Type.ToString().ToLower().Contains("list")) &&
                    (f.ToString().ToLower().Contains("data") ||
                     f.ToString().ToLower().Contains("content") ||
                     f.ToString().ToLower().Contains("array")));

                if (hasFileName && hasArrayData)
                {
                    MetCriteria++;
                    CriteriaDetails.Add("✅ Приватные поля с именем файла и массивом содержимого найдены");
                    return;
                }
            }

            CriteriaDetails.Add("❌ Не найдены оба приватных поля: имя файла и массив содержимого");
        }

        private static void CheckReadFromFileMethod(SyntaxNode root)
        {
            var methodNodes = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var method in methodNodes)
            {
                var text = method.ToString().ToLower();
                if (
                    (text.Contains("read") || text.Contains("stream") || text.Contains("file")) &&
                    (text.Contains("array") || text.Contains("list") || text.Contains("[]"))
                )
                {
                    MetCriteria++;
                    CriteriaDetails.Add("✅ Метод чтения из файла в массив найден");
                    return;
                }
            }

            CriteriaDetails.Add("❌ Метод чтения из файла в массив не найден");
        }

        private static void CheckConsoleOutputMethod(SyntaxNode root)
        {
            var methodNodes = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var method in methodNodes)
            {
                var text = method.ToString().ToLower();
                if (text.Contains("console.write") || text.Contains("console.writeline"))
                {
                    MetCriteria++;
                    CriteriaDetails.Add("✅ Метод вывода содержимого на консоль найден");
                    return;
                }
            }

            CriteriaDetails.Add("❌ Метод вывода на консоль не найден");
        }

        private static void CheckIsAllNumbersMethod(SyntaxNode root)
        {
            var methodNodes = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var method in methodNodes)
            {
                var returnsBool = method.ReturnType.ToString().ToLower() == "bool";
                var text = method.ToString().ToLower();
                var hasParsing = text.Contains("int.parse") || text.Contains("double.parse") || text.Contains("tryparse");

                if (returnsBool && hasParsing)
                {
                    MetCriteria++;
                    CriteriaDetails.Add("✅ Метод проверки 'только ли числа' найден");
                    return;
                }
            }

            CriteriaDetails.Add("❌ Метод проверки, что только числа, не найден");
        }

        private static void CheckSumIfAllNumbersMethod(SyntaxNode root)
        {
            var methodNodes = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var method in methodNodes)
            {
                var text = method.ToString().ToLower();

                if (
                    (text.Contains("sum") || text.Contains("+=")) &&
                    (text.Contains("if") && text.Contains("is") && text.Contains("number")) || // примерный фильтр
                    text.Contains("if") && text.Contains("parse") ||
                    text.Contains("tryparse")
                )
                {
                    MetCriteria++;
                    CriteriaDetails.Add("✅ Метод подсчёта суммы чисел при условии корректности найден");
                    return;
                }
            }

            CriteriaDetails.Add("❌ Метод подсчёта суммы при условии 'только числа' не найден");
        }

        private static string PrintResults()
        {
            var result = $"Выполнено критериев: {MetCriteria} из {TotalCriteria}\nДетали:\n";
            foreach (var detail in CriteriaDetails)
                result += detail + "\n";
            return result;
        }
    }
}