using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task18
    {
        public static int TotalCriteria { get; set; } = 6;
        public static int MetCriteria { get; set; } = 0;
        public static List<string> CriteriaDetails { get; } = new();

        public static (string totalScore, string criteria) AnalyzeFromText(string fullText)
        {
            var tree = CSharpSyntaxTree.ParseText(fullText);
            var root = tree.GetRoot();
            return ($"{MetCriteria}/{TotalCriteria}", Analyze(new List<SyntaxNode> { root }));
        }

        public static string Analyze(List<SyntaxNode> roots)
        {
            MetCriteria = 0;
            CriteriaDetails.Clear();

            CheckQuadrilateralClass(roots);
            CheckDerivedRectangleClass(roots);
            CheckComputationMethods(roots);
            CheckRectangleValidationMethod(roots);
            CheckOutputMethod(roots);
            CheckTests(roots);

            return PrintResults();
        }

        private static void CheckQuadrilateralClass(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var classDecls = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
                foreach (var cls in classDecls)
                {
                    if (!cls.Identifier.Text.ToLower().Contains("quad"))
                        continue;

                    var hasArrayOfTuples = cls.Members
                        .OfType<FieldDeclarationSyntax>()
                        .Any(f => f.Declaration.Type.ToString().Contains("(double x, double y)["));

                    if (hasArrayOfTuples)
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден класс четырёхугольника с массивом 4 координат.");
                        return;
                    }

                    var coords = cls.Members
                        .OfType<FieldDeclarationSyntax>()
                        .Concat<MemberDeclarationSyntax>(cls.Members.OfType<PropertyDeclarationSyntax>())
                        .Where(m => m.ToString().ToLower().Contains("x") && m.ToString().ToLower().Contains("y"));

                    if (coords.Count() >= 4)
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден класс четырёхугольника с 4 отдельными координатами.");
                        return;
                    }
                }
            }
            CriteriaDetails.Add("❌ Класс четырёхугольника с 4 координатами не найден.");
        }

        private static void CheckDerivedRectangleClass(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
                foreach (var cls in classes)
                {
                    if (cls.Identifier.Text.ToLower().Contains("rect") &&
                        cls.BaseList?.Types.Any() == true &&
                        cls.BaseList.Types.ToString().ToLower().Contains("quad"))
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден производный класс прямоугольника.");
                        return;
                    }
                }
            }
            CriteriaDetails.Add("❌ Производный класс прямоугольника не найден.");
        }

        private static void CheckComputationMethods(List<SyntaxNode> roots)
        {
            bool hasSides = false, hasDiagonals = false, hasPerimeter = false, hasArea = false;

            foreach (var root in roots)
            {
                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

                foreach (var method in methods)
                {
                    var name = method.Identifier.Text.ToLower();
                    var body = method.ToString().ToLower();

                    if (!hasSides && name.Contains("side")) hasSides = true;
                    if (!hasDiagonals && name.Contains("diagon")) hasDiagonals = true;
                    if (!hasPerimeter && name.Contains("perim")) hasPerimeter = true;
                    if (!hasArea && (name.Contains("area") || name.Contains("square"))) hasArea = true;

                    if (hasSides && hasDiagonals && hasPerimeter && hasArea)
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найдены методы вычисления: сторон, диагоналей, периметра, площади.");
                        return;
                    }
                }
            }
            CriteriaDetails.Add("❌ Не все методы вычисления (стороны, диагонали, периметр, площадь) найдены.");
        }

        private static void CheckRectangleValidationMethod(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (var method in methods)
                {
                    if (method.Identifier.Text.ToLower().Contains("is") &&
                        method.Identifier.Text.ToLower().Contains("rect"))
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден метод проверки, является ли фигура прямоугольником.");
                        return;
                    }
                }
            }
            CriteriaDetails.Add("❌ Метод проверки прямоугольника не найден.");
        }

        private static void CheckOutputMethod(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (var method in methods)
                {
                    var body = method.ToString().ToLower();
                    if (body.Contains("console.write") || body.Contains("return"))
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден метод вывода сведений о фигуре.");
                        return;
                    }
                }
            }
            CriteriaDetails.Add("❌ Метод вывода сведений о фигуре не найден.");
        }

        private static void CheckTests(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

                foreach (var method in methods)
                {
                    var isTestName = method.Identifier.Text.ToLower().StartsWith("test");
                    var containsAssertApprox = method.DescendantNodes()
                        .OfType<InvocationExpressionSyntax>()
                        .Any(inv => inv.ToString().ToLower().Contains("assertapprox"));

                    if (isTestName && containsAssertApprox)
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден пользовательский тест с AssertApprox.");
                        return;
                    }

                    var attributes = method.AttributeLists.SelectMany(a => a.Attributes).ToList();
                    var hasTestAttribute = attributes.Any(attr =>
                    {
                        var name = attr.Name.ToString().ToLower();
                        return name.Contains("test") || name.Contains("fact") || name.Contains("theory");
                    });

                    var hasAssert = method.DescendantNodes()
                        .OfType<InvocationExpressionSyntax>()
                        .Any(inv => inv.Expression.ToString().ToLower().Contains("assert"));

                    if (hasTestAttribute && hasAssert)
                    {
                        MetCriteria++;
                        CriteriaDetails.Add("✅ Найден фреймворковый тест с атрибутом и Assert.");
                        return;
                    }
                }
            }
            CriteriaDetails.Add("❌ Тесты не найдены или не распознаны.");
        }

        public static string PrintResults()
        {
            var result = $"Выполнено критериев: {MetCriteria} из {TotalCriteria}\nДетали:\n";
            result += string.Join("\n", CriteriaDetails);
            return result;
        }
    }
}