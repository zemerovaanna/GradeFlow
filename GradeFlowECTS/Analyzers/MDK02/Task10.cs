using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task10
    {
        public static int TotalCriteria { get; set; } = 5;
        public static int MetCriteria { get; set; }
        public static List<string> CriteriaDetails { get; } = new();

        public static string Analyze(SyntaxNode root)
        {
            MetCriteria = 0;
            CriteriaDetails.Clear();

            CheckTriangleClass(root);
            CheckExistenceMethod(root);
            CheckPerimeterAndAreaMethods(root);
            CheckTriangleTests(root);

            return PrintResults();
        }

        private static void CheckTriangleClass(SyntaxNode root)
        {
            var triangleClass = root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(cls => cls.Identifier.Text.ToLower().Contains("triangle") ||
                                       cls.Members.OfType<FieldDeclarationSyntax>()
                                           .Any(f => f.ToString().ToLower().Contains("side")));

            if (triangleClass != null)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Класс треугольника найден");
            }
            else
            {
                CriteriaDetails.Add("❌ Класс треугольника не найден");
            }
        }

        private static void CheckExistenceMethod(SyntaxNode root)
        {
            var found = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Any(m =>
                {
                    var body = m.ToString().ToLower();
                    return (m.Identifier.Text.ToLower().Contains("exist") ||
                            m.Identifier.Text.ToLower().Contains("valid") ||
                            m.Identifier.Text.ToLower().Contains("triangle")) &&
                           (body.Contains("a + b > c") || body.Contains("&&"));
                });

            if (found)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Метод проверки существования треугольника найден");
            }
            else
            {
                CriteriaDetails.Add("❌ Метод проверки существования треугольника не найден");
            }
        }

        private static void CheckPerimeterAndAreaMethods(SyntaxNode root)
        {
            bool hasPerimeter = false;
            bool hasArea = false;

            var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var method in methods)
            {
                var text = method.ToString().ToLower();
                if (text.Contains("perimeter") || text.Contains("a + b + c"))
                    hasPerimeter = true;

                if (text.Contains("area") || text.Contains("heron") || text.Contains("math.sqrt"))
                    hasArea = true;
            }

            if (hasPerimeter)
            {
                CriteriaDetails.Add("✅ Метод расчёта периметра найден");
                MetCriteria++;
            }
            else
            {
                CriteriaDetails.Add("❌ Метод расчёта периметра не найден");
            }

            if (hasArea)
            {
                CriteriaDetails.Add("✅ Метод расчёта площади найден");
                MetCriteria++;
            }
            else
            {
                CriteriaDetails.Add("❌ Метод расчёта площади не найден");
            }
        }

        private static void CheckTriangleTests(SyntaxNode root)
        {
            var testClasses = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Where(cls => cls.Identifier.Text.ToLower().Contains("test") ||
                              cls.AttributeLists.SelectMany(a => a.Attributes)
                                  .Any(attr => attr.Name.ToString().ToLower().Contains("test")));

            bool hasAssert = false;
            bool testsTriangle = false;

            foreach (var testClass in testClasses)
            {
                var methods = testClass.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (var method in methods)
                {
                    var text = method.ToString().ToLower();
                    if (text.Contains("assert"))
                        hasAssert = true;

                    if (text.Contains("triangle") || text.Contains("area") || text.Contains("perimeter"))
                        testsTriangle = true;
                }
            }

            if (hasAssert && testsTriangle)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Найдены тесты для треугольника");
            }
            else
            {
                CriteriaDetails.Add("❌ Не найдены тесты для треугольника");
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