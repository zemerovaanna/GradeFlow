using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.MDK02
{
    public static class Task7
    {
        public static int TotalCriteria { get; set; } = 7;
        public static int MetCriteria { get; set; } = 0;
        public static List<string> CriteriaDetails { get; } = new();

        public static string Analyze(SyntaxNode root)
        {
            MetCriteria = 0;
            CriteriaDetails.Clear();

            CheckHumanBaseClass(root);
            CheckStudentClass(root);
            CheckTeacherClass(root);
            CheckTests(root);

            return PrintResults();
        }

        private static void CheckHumanBaseClass(SyntaxNode root)
        {
            var humanClass = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(c => c.Identifier.Text.ToLower().Contains("chelovek") || c.Identifier.Text.ToLower().Contains("human"));

            if (humanClass == null)
            {
                CriteriaDetails.Add("❌ Класс 'Человек' не найден");
                return;
            }

            // Критерий 1: Есть поля фио и адрес
            var fields = humanClass.Members.OfType<FieldDeclarationSyntax>()
                .Select(f => f.ToString().ToLower()).ToList();

            bool hasFio = fields.Any(f => f.Contains("fio") || f.Contains("name"));
            bool hasAddress = fields.Any(f => f.Contains("adres") || f.Contains("address"));

            if (hasFio && hasAddress)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Класс 'Человек' содержит ФИО и адрес");
            }
            else
            {
                CriteriaDetails.Add("❌ Класс 'Человек' не содержит корректные поля ФИО и адрес");
            }

            // Критерий 2: Info возвращает ФИО
            var infoMethod = humanClass.Members.OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.Identifier.Text.ToLower().Contains("info"));

            if (infoMethod != null && infoMethod.ReturnType.ToString().ToLower().Contains("string"))
            {
                if (infoMethod.ToString().ToLower().Contains("fio") || infoMethod.ToString().ToLower().Contains("name"))
                {
                    MetCriteria++;
                    CriteriaDetails.Add("✅ Метод Info() класса 'Человек' возвращает ФИО");
                }
                else
                {
                    CriteriaDetails.Add("❌ Метод Info() найден, но не возвращает ФИО");
                }
            }
            else
            {
                CriteriaDetails.Add("❌ Метод Info() не найден в классе 'Человек'");
            }
        }

        private static void CheckStudentClass(SyntaxNode root)
        {
            var studentClass = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(c => c.Identifier.Text.ToLower().Contains("student"));

            if (studentClass == null)
            {
                CriteriaDetails.Add("❌ Класс 'Студент' не найден");
                return;
            }

            // Критерий 3: наследуется от Человека + есть учебное заведение
            var baseList = studentClass.BaseList?.ToString().ToLower();
            bool inheritsHuman = baseList != null &&
                (baseList.Contains("chelovek") || baseList.Contains("human"));

            var fields = studentClass.Members.OfType<FieldDeclarationSyntax>()
                .Select(f => f.ToString().ToLower()).ToList();

            bool hasUniversity = fields.Any(f => f.Contains("univ") || f.Contains("zaved") || f.Contains("school"));

            if (inheritsHuman && hasUniversity)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Класс 'Студент' наследует 'Человек' и содержит учебное заведение");
            }
            else
            {
                CriteriaDetails.Add("❌ Класс 'Студент' не наследует 'Человек' или не содержит учебное заведение");
            }

            // Критерий 4: Info() переопределён и содержит ФИО + вуз
            var infoMethod = studentClass.Members.OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.Identifier.Text.ToLower().Contains("info"));

            if (infoMethod != null &&
                infoMethod.Modifiers.Any(m => m.Text.ToLower() == "override") &&
                (infoMethod.ToString().ToLower().Contains("fio") || infoMethod.ToString().ToLower().Contains("name")) &&
                (infoMethod.ToString().ToLower().Contains("univ") || infoMethod.ToString().ToLower().Contains("zaved")))
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Метод Info() класса 'Студент' переопределён и содержит ФИО + вуз");
            }
            else
            {
                CriteriaDetails.Add("❌ Метод Info() в классе 'Студент' не соответствует требованиям");
            }
        }

        private static void CheckTeacherClass(SyntaxNode root)
        {
            var teacherClass = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(c => c.Identifier.Text.ToLower().Contains("prepod") || c.Identifier.Text.ToLower().Contains("teacher"));

            if (teacherClass == null)
            {
                CriteriaDetails.Add("❌ Класс 'Преподаватель' не найден");
                return;
            }

            // Критерий 5: наследует 'Студент' и содержит учёную степень
            var baseList = teacherClass.BaseList?.ToString().ToLower();
            bool inheritsStudent = baseList != null && baseList.Contains("student");

            var fields = teacherClass.Members.OfType<FieldDeclarationSyntax>()
                .Select(f => f.ToString().ToLower()).ToList();

            bool hasDegree = fields.Any(f => f.Contains("stepen") || f.Contains("degree"));

            if (inheritsStudent && hasDegree)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Класс 'Преподаватель' наследует 'Студент' и содержит учёную степень");
            }
            else
            {
                CriteriaDetails.Add("❌ Класс 'Преподаватель' не соответствует требованиям по наследованию/степени");
            }

            // Критерий 6: Info() переопределён и содержит ФИО + вуз + степень
            var infoMethod = teacherClass.Members.OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.Identifier.Text.ToLower().Contains("info"));

            if (infoMethod != null &&
                infoMethod.Modifiers.Any(m => m.Text.ToLower() == "override") &&
                infoMethod.ToString().ToLower().Contains("fio") &&
                infoMethod.ToString().ToLower().Contains("univ") &&
                infoMethod.ToString().ToLower().Contains("stepen"))
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Метод Info() класса 'Преподаватель' переопределён и содержит ФИО + вуз + степень");
            }
            else
            {
                CriteriaDetails.Add("❌ Метод Info() в классе 'Преподаватель' не соответствует требованиям");
            }
        }

        private static void CheckTests(SyntaxNode root)
        {
            var testMethods = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Where(m =>
                    m.Identifier.Text.ToLower().Contains("test") &&
                    m.ToString().ToLower().Contains("info")
                ).ToList();

            if (testMethods.Count >= 3)
            {
                MetCriteria++;
                CriteriaDetails.Add("✅ Присутствуют тесты для метода Info() в трёх классах");
            }
            else
            {
                CriteriaDetails.Add("❌ Недостаточно тестов для метода Info()");
            }
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