using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task3
    {
        public static int TotalCriteria { get; set; } = 3;
        public static int MetCriteria { get; set; } = 0;
        public static List<string> CriteriaDetails { get; } = new();

        public static (string totalScore, string criteria) Analyze(SyntaxNode root)
        {
            MetCriteria = 0;
            CriteriaDetails.Clear();

            CheckTestClassPresence(root);
            CheckFileBasedTestLogic(root);
            CheckCharacterCountAssertions(root);

            return ($"{MetCriteria}/{TotalCriteria}", PrintResults());
        }

        private static void CheckTestClassPresence(SyntaxNode root)
        {
            var testClasses = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Where(cls =>
                    cls.AttributeLists.SelectMany(a => a.Attributes)
                        .Any(attr => attr.Name.ToString().ToLower().Contains("test")) ||
                    cls.Identifier.Text.ToLower().Contains("test"));

            if (testClasses.Any())
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Найден класс с тестами");
            }
            else
            {
                CriteriaDetails.Add("❌ Класс с тестами не найден");
            }
        }

        private static void CheckFileBasedTestLogic(SyntaxNode root)
        {
            var methodNodes = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var method in methodNodes)
            {
                var text = method.ToString().ToLower();

                bool usesFile = text.Contains("file") || text.Contains("streamreader") || text.Contains("readalltext");

                if (usesFile)
                {
                    MetCriteria++;
                    CriteriaDetails.Add("✅ В тестах используется файл или его содержимое");
                    return;
                }
            }

            CriteriaDetails.Add("❌ В тестах не обнаружено использование файлов");
        }

        private static void CheckCharacterCountAssertions(SyntaxNode root)
        {
            var methodNodes = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var method in methodNodes)
            {
                var text = method.ToString().ToLower();

                bool hasAssert = text.Contains("assert");
                bool checksCount = text.Contains("length") || text.Contains("count") || text.Contains(".length");

                if (hasAssert && checksCount)
                {
                    MetCriteria++;
                    CriteriaDetails.Add("✅ Найдён assert, проверяющий количество символов");
                    return;
                }
            }

            CriteriaDetails.Add("❌ Assert проверки количества символов не найден");
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