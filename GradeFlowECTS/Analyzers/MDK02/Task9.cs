using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task9
    {
        public static int TotalCriteria { get; set; } = 5;
        public static int MetCriteria { get; set; }
        public static List<string> CriteriaDetails { get; } = new();

        public static string Analyze(SyntaxNode root)
        {
            MetCriteria = 0;
            CriteriaDetails.Clear();

            CheckStringProcessorClass(root);
            CheckWordCountMethod(root);
            CheckToLowerMethod(root);
            CheckLetterCountMethod(root);
            CheckTestsForMethods(root);

            return PrintResults();
        }

        private static void CheckStringProcessorClass(SyntaxNode root)
        {
            var classNode = root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(cls => cls.Identifier.Text.ToLower().Contains("string"));

            if (classNode != null)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Найден класс обработки строк");
            }
            else
            {
                CriteriaDetails.Add("❌ Класс обработки строк не найден");
            }
        }

        private static void CheckWordCountMethod(SyntaxNode root)
        {
            var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
            var found = methods.Any(m =>
            {
                var name = m.Identifier.Text.ToLower();
                var body = m.ToString().ToLower();
                return (name.Contains("word") || name.Contains("count")) &&
                       body.Contains(".split") &&
                       (body.Contains(" ") || body.Contains("stringsplitoptions"));
            });

            if (found)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Метод подсчёта слов найден");
            }
            else
            {
                CriteriaDetails.Add("❌ Метод подсчёта слов не найден");
            }
        }

        private static void CheckToLowerMethod(SyntaxNode root)
        {
            var found = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Any(m => m.ToString().ToLower().Contains(".tolower"));

            if (found)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Метод замены заглавных на строчные найден");
            }
            else
            {
                CriteriaDetails.Add("❌ Метод замены заглавных на строчные не найден");
            }
        }

        private static void CheckLetterCountMethod(SyntaxNode root)
        {
            var found = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Any(m =>
                {
                    var body = m.ToString().ToLower();
                    return (body.Contains("char.isletter") || body.Contains("isletter")) &&
                           body.Contains("foreach") || body.Contains(".count");
                });

            if (found)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Метод подсчёта букв найден");
            }
            else
            {
                CriteriaDetails.Add("❌ Метод подсчёта букв не найден");
            }
        }

        private static void CheckTestsForMethods(SyntaxNode root)
        {
            var testClasses = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Where(cls => cls.Identifier.Text.ToLower().Contains("test") ||
                              cls.AttributeLists.SelectMany(a => a.Attributes)
                                  .Any(attr => attr.Name.ToString().ToLower().Contains("test")));

            bool hasAssert = false;
            bool testsStringMethods = false;

            foreach (var testClass in testClasses)
            {
                var methods = testClass.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (var method in methods)
                {
                    var text = method.ToString().ToLower();
                    if (text.Contains("assert"))
                        hasAssert = true;

                    if (text.Contains("word") || text.Contains("tolower") || text.Contains("letter"))
                        testsStringMethods = true;
                }
            }

            if (hasAssert && testsStringMethods)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Найдены тесты для методов обработки строк");
            }
            else
            {
                CriteriaDetails.Add("❌ Не найдены тесты для методов обработки строк");
            }
        }

        public static string PrintResults()
        {
            var result = $"Выполнено критериев: {MetCriteria} из {TotalCriteria}\nДетали:\n";
            foreach (var detail in CriteriaDetails)
                result += detail + "\n";
            return result;
        }
    }
}