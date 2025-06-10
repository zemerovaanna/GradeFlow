using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task17
    {
        public static int TotalCriteria { get; set; } = 5;
        public static int MetCriteria { get; set; } = 0;
        public static List<string> CriteriaDetails { get; } = new();

        public static string AnalyzeFromText(string fullText)
        {
            var tree = CSharpSyntaxTree.ParseText(fullText);
            var root = tree.GetRoot();
            return Analyze(new List<SyntaxNode> { root });
        }

        public static string Analyze(List<SyntaxNode> roots)
        {
            MetCriteria = 0;
            CriteriaDetails.Clear();

            CheckRobotBaseClass(roots);
            CheckSuperRobot(roots);
            CheckSuperPuperRobot(roots);
            CheckPolymorphicCalls(roots);
            CheckTests(roots);

            return PrintResults();
        }

        private static void CheckRobotBaseClass(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var robotClass = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                    .FirstOrDefault(c => c.Identifier.Text.ToLower() == "robot");

                if (robotClass != null)
                {
                    var methods = robotClass.Members.OfType<MethodDeclarationSyntax>();
                    var calc = methods.FirstOrDefault(m => m.Identifier.Text.ToLower().Contains("calc"));
                    if (calc != null && calc.Modifiers.Any(m => m.Text == "virtual"))
                    {
                        var body = calc.ToString().ToLower();
                        if (body.Contains("+") || body.Contains("-"))
                        {
                            MetCriteria++;
                            CriteriaDetails.Add("✅ Найден базовый класс с методом Счет (+, -).");
                            return;
                        }
                    }
                }
            }
            CriteriaDetails.Add("❌ Базовый класс Робот с методом Счет не найден.");
        }

        private static void CheckSuperRobot(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var superRobot = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                    .FirstOrDefault(c => c.Identifier.Text.ToLower().Contains("superrobot") &&
                                         c.BaseList?.Types.Any(t => t.ToString().ToLower().Contains("robot")) == true);

                if (superRobot != null)
                {
                    var methods = superRobot.Members.OfType<MethodDeclarationSyntax>()
                        .Where(m => m.Identifier.Text.ToLower().Contains("calc") && m.Modifiers.Any(m => m.Text == "override"));

                    foreach (var method in methods)
                    {
                        var body = method.ToString().ToLower();
                        if (body.Contains("*") || body.Contains("/"))
                        {
                            MetCriteria++;
                            CriteriaDetails.Add("✅ Найден наследник, переопределяющий Счет для * и /.");
                            return;
                        }
                    }
                }
            }
            CriteriaDetails.Add("❌ Класс СуперРобот с нужным функционалом не найден.");
        }

        private static void CheckSuperPuperRobot(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                    .Where(c => c.BaseList != null && c.BaseList.Types.Any(t => t.ToString().ToLower().Contains("superrobot")));

                foreach (var cls in classes)
                {
                    var method = cls.Members.OfType<MethodDeclarationSyntax>()
                        .FirstOrDefault(m => m.Identifier.Text.ToLower().Contains("calc"));

                    if (method != null)
                    {
                        var body = method.ToString().ToLower();
                        if (body.Contains("datatable") || body.Contains("compute"))
                        {
                            MetCriteria++;
                            CriteriaDetails.Add("✅ Найден класс СуперПуперРобот с полной арифметикой (через DataTable).");
                            return;
                        }

                        if (body.Contains("%") || body.Contains("(")) // бонус: модуль и скобки
                        {
                            MetCriteria++;
                            CriteriaDetails.Add("✅ Найден класс СуперПуперРобот с полной арифметикой (по содержанию тела метода).");
                            return;
                        }
                    }
                }
            }
            CriteriaDetails.Add("❌ Класс СуперПуперРобот с полной арифметикой не найден.");
        }

/*        private static void CheckPolymorphicCalls(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                var variables = root.DescendantNodes().OfType<VariableDeclarationSyntax>();

                foreach (var declaration in variables)
                {
                    var typeName = declaration.Type.ToString().ToLower();
                    if (typeName.Contains("robot"))
                    {
                        foreach (var variable in declaration.Variables)
                        {
                            if (variable.Initializer?.Value is ObjectCreationExpressionSyntax creation)
                            {
                                var createdType = creation.Type.ToString().ToLower();
                                if (createdType != typeName && createdType.Contains("robot"))
                                {
                                    MetCriteria++;
                                    CriteriaDetails.Add("✅ Обнаружен полиморфный вызов метода Счет.");
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            CriteriaDetails.Add("❌ Использование полиморфного вызова метода Счет не обнаружено.");
        }*/

        private static void CheckPolymorphicCalls(List<SyntaxNode> roots)
        {
            foreach (var root in roots)
            {
                // Получаем все вызовы методов
                var invocations = root.DescendantNodes().OfType<InvocationExpressionSyntax>();

                foreach (var invocation in invocations)
                {
                    // Пытаемся получить выражение, на котором вызывается метод (например, robot.Calculate)
                    if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
                    {
                        // Проверяем имя метода
                        var methodName = memberAccess.Name.Identifier.Text;
                        if (methodName != "Calculate")
                            continue;

                        // Получаем семантическую модель для определения типа (нужна для полноценного анализа, но сейчас - простая проверка)
                        // Для твоего контекста возможно нужна передача SemanticModel в метод - добавь, если надо.
                        // Здесь делаем упрощенную проверку на имя типа переменной

                        var receiver = memberAccess.Expression; // выражение слева от точки

                        // Тип переменной — простой способ: взять текст выражения (может быть переменная)
                        var typeName = receiver.ToString().ToLower();

                        // Чтобы отловить вызов через базовый тип Robot, проверим на "robot" в названии переменной.
                        // Это упрощение, лучше использовать SemanticModel для точного определения типа.

                        if (typeName.Contains("robot"))
                        {
                            // Нашли вызов Calculate через переменную с "robot" в имени — считаем полиморфным
                            MetCriteria++;
                            CriteriaDetails.Add("✅ Обнаружен полиморфный вызов метода Счет.");
                            return;
                        }
                    }
                }
            }
            CriteriaDetails.Add("❌ Использование полиморфного вызова метода Счет не обнаружено.");
        }

        private static void CheckTests(List<SyntaxNode> roots)
        {
            bool robotTested = false;
            bool superRobotTested = false;
            bool superPuperRobotTested = false;

            foreach (var root in roots)
            {
                var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

                foreach (var method in methodDeclarations)
                {
                    var attributes = method.AttributeLists.SelectMany(al => al.Attributes).ToList();
                    bool hasTestAttribute = attributes.Any(attr =>
                    {
                        var name = attr.Name.ToString().ToLower();
                        return name.Contains("test") || name.Contains("fact") || name.Contains("theory");
                    });

                    if (!hasTestAttribute)
                        continue;

                    var methodText = method.ToString().ToLower();
                    if (methodText.Contains("new robot(") || methodText.Contains("robot();"))
                        if (methodText.Contains(".calculate("))
                            robotTested = true;

                    if (methodText.Contains("new superrobot(") || methodText.Contains("superrobot();"))
                        if (methodText.Contains(".calculate("))
                            superRobotTested = true;

                    if (methodText.Contains("new superpuperrobot(") || methodText.Contains("superpuperrobot();"))
                        if (methodText.Contains(".calculate("))
                            superPuperRobotTested = true;
                }
            }

            if (robotTested)
                CriteriaDetails.Add("✅ Присутствует тест для класса Robot с методом Calculate.");
            else
                CriteriaDetails.Add("❌ Нет теста для класса Robot с методом Calculate.");

            if (superRobotTested)
                CriteriaDetails.Add("✅ Присутствует тест для класса SuperRobot с методом Calculate.");
            else
                CriteriaDetails.Add("❌ Нет теста для класса SuperRobot с методом Calculate.");

            if (superPuperRobotTested)
                CriteriaDetails.Add("✅ Присутствует тест для класса SuperPuperRobot с методом Calculate.");
            else
                CriteriaDetails.Add("❌ Нет теста для класса SuperPuperRobot с методом Calculate.");

            if (robotTested && superRobotTested && superPuperRobotTested)
                MetCriteria++;
        }

        public static string PrintResults()
        {
            string result = $"Выполнено критериев: {MetCriteria} из {TotalCriteria}\nДетали:\n";
            foreach (var detail in CriteriaDetails)
            {
                result += detail + "\n";
            }
            return result;
        }
    }
}