using System.Security.Cryptography;
using System.Text;
using System.Windows;
using GradeFlowECTS.Analyzers.MDK02;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GradeFlowECTS.View.Windows
{
    public partial class StudentMDK02Window : Window
    {
        private List<(byte Number, string TaskText)> _tasks;
        private (byte Number, string TaskText) _task;
        private int _studentId;
        private Guid _examId;

        public StudentMDK02Window(int studentId, Guid examId)
        {
            InitializeComponent();

            _studentId = studentId;
            _examId = examId;

            _tasks = new()
            {
                (1, "Разработать класс квадратное уравнение с соответствующим методом поиска корней. Разработать автоматизированные тесты для программы вычисления дискриминанта квадратного уравнения (В*В-4*А*С)."),
                (2, "Разработать класс график, с методом ВЫЧИСЛЕНИЯ ЗНАЧЕНИЙ ГРАФИКА ФУНКЦИИ У=1/(Х-4) НА ВВЕДЕННОМ ОТРЕЗКЕ [A,B] и автоматизированные тесты к методу."),
                (3, "Разработать тесты черным ящиком для программы: Считающей количество символов в заданном файле."),
                (4, "Разработать соответствующий класс Матрица и метод, считающую количество ненулевых элементов квадратной матрицы (0<n<10). Элементы матрицы от 10 до 20. Вся информация о размере и содержимом матрицы вводится с клавиатуры."),
                (5, "Разработать класс график, с методом ВЫЧИСЛЕНИЯ ЗНАЧЕНИЙ ГРАФИКА ГРАФИКА ФУНКЦИИ У=1/ НА ВВЕДЕННОМ ОТРЕЗКЕ [C,D]. Написать тесты входных данных на программу."),
                (6, "Разработать и описать класс, содержащий приватные поля, включающие информацию о имени файла, массиве с содержимым данного файла, методах чтения из файла в массив и вывода на консоль, проверки только ли числа в файле с возвратом значения логического типа и методом нахождения суммы если только числа в файле."),
                (7, "Разработать и описать родительский класс Человек с ФИО и адресом, метод info() для этого класса возвращает фио. Его подчиненный класс Студент с информацией о учебном заведении которое закончил - метод инфо возвращает строку с фио и названием учебного заведения, подчиненный класс для данного класса Преподаватель с названием ученой степени, и методами Инфо для каждого класса своими, использовать переопределение методов. Реализовать тесты к методу info()."),
                (8, "Разработать класс, содержащий приватные поля и методы работы с парами чисел А и В:\r\n-подсчет суммы А и В\r\n-возведение А в степень В\r\n-подсчет частного А и В\r\n-вывод на экран А раз числа В\r\nРазработать тесты к методам"),
                (9, "Разработать класс, содержащий методы работы со строками\r\n-подсчет слов\r\n-замена заглавных на строчные\r\n-подсчет количества букв.\r\nРазработать тесты к методам"),
                (10, "Разработать и создать класс треугольник, члены класса - длины 3-х сторон. Предусмотреть в классе методы проверки существования треугольника, вычисления и вывода сведений о фигуре – длины сторон, периметр, площадь. Разработать тесты к методам."),
                (11, "Реализовать класс Счет. Разработать автоматизированные тесты для программы, считающей сумму цифр во введенной строке."),
                (12, "Реализовать класс Счет. Разработать автоматизированные тесты для программы, считающей произведение цифр в строке, считанной из файла."),
                (13, "Разработать автоматизированные тесты для программы поиска минимального элемента в матрице, которая вводится с клавиатуры."),
                (14, "Создать класс, объектами с двумя переменными и знаком операции. Операциями могут быть +,-,/,*, ^(степень), хранимые в файле. Реализовать метод добавления данных в файл. Добавить функцию вывода на экран значений переменных и функцию вычисления операции над переменными и функцию которая находит наибольшее значение из этих двух переменных."),
                (15, "Создать класс реализующий следующие действия со строкой:\r\n● Возврат строки с разбиением на слова;\r\n● Подсчет количества слов в строке;\r\n● Проверка наличия зааданного слова.\r\n● Реализовать тесты."),
                (16, "Создайте класс,реализующий следующие действия с двумерным целочисленным\r\nмассивом:\r\n● инициализация, ввод элементов,\r\n● вывод элементов\r\n● нахождение суммы по строкам\r\n● Реализовать тесты."),
                (17, "Создайте классы, реализующие следующие действия с двумерным целочисленным массивом:\r\n● класс Робот-базовый класс\r\n○ метод Счет\r\n○ входные данные метода - строка с примером на сложение или вычитание,\r\n○ вида 3+4 или 5-6 выходное данное- число int\r\n● наследник от базового класс СуперРобот\r\n○ Метод Счет Переопределен, на вход подается строка вида число1*число2\r\n(например 3*45 ) или число1/число 2 (63/ 9) результат int\r\n● наследник от СуперРобот - класс СуперПуперРобот\r\nМетод Счет -переопределен может принимать строку с любым арифметическим\r\nвыражением, те такую, что может вычислить любую из 5 операций:\r\n● 1+2\r\n● 3-2\r\n● 3*4\r\n● 5/8\r\n● 13%3\r\nавтотесты к методам."),
                (18, "Создать класс четырехугольник, члены класса - координаты 4-х точек. Предусмотреть в классе вычисления и вывода сведений о фигуре - длины сторон, диагоналей, периметр,площадь. Создать производный класс прямоугольник, предусмотреть в классе проверку, является ли фигура прямоугольником. Реализовать тесты к методам.")
            };

            var random = new Random();
            int index = random.Next(_tasks.Count);
            _task = _tasks[index];
            TaskText.Text = $"{_task.Number}. " + _task.TaskText;
        }

        private (string totalScore, string criteria) AnalyzeCode(string code, int taskNumber)
        {
            try
            {
                SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
                var root = tree.GetRoot();

                switch (taskNumber)
                {
                    case 1:
                        return (Task1.Analyze(root).totalScore, Task1.Analyze(root).criteria);
                    case 2:
                        return (Task2.Analyze(root).totalScore, Task2.Analyze(root).criteria);
                    case 3:
                        return (Task3.Analyze(root).totalScore, Task3.Analyze(root).criteria);
                    case 4:
                        return (Task4.Analyze(root).totalScore, Task4.Analyze(root).criteria);
                    case 5:
                        return (Task5.Analyze(root).totalScore, Task5.Analyze(root).criteria);
                    case 6:
                        return (Task6.Analyze(root).totalScore, Task6.Analyze(root).criteria);
                    case 7:
                        return (Task7.Analyze(root).totalScore, Task7.Analyze(root).criteria);
                    case 8:
                        return (Task8.Analyze(root).totalScore, Task8.Analyze(root).criteria);
                    case 9:
                        return (Task9.Analyze(root).totalScore, Task9.Analyze(root).criteria);
                    case 10:
                        return (Task10.Analyze(root).totalScore, Task10.Analyze(root).criteria);
                    case 11:
                        return (Task11.Analyze(root).totalScore, Task11.Analyze(root).criteria);
                    case 12:
                        return (Task12.Analyze(root).totalScore, Task12.Analyze(root).criteria);
                    case 13:
                        return (Task13.Analyze(root).totalScore, Task13.Analyze(root).criteria);
                    case 14:
                        return (Task14.AnalyzeFromText(code).totalScore, Task14.AnalyzeFromText(code).criteria);
                    case 15:
                        return (Task15.AnalyzeFromText(code).totalScore, Task15.AnalyzeFromText(code).criteria);
                    case 16:
                        return (Task16.AnalyzeFromText(code).totalScore, Task16.AnalyzeFromText(code).criteria);
                    case 17:
                        return (Task17.AnalyzeFromText(code).totalScore, Task17.AnalyzeFromText(code).criteria);
                    case 18:
                        return (Task18.AnalyzeFromText(code).totalScore, Task18.AnalyzeFromText(code).criteria);
                    default:
                        return ("0/0", "Неверный номер задания.");
                }
            }
            catch (Exception ex)
            {
                return ("0/0", "Ошибка анализа: " + ex.Message);
            }
        }

        /*        private void CheckCode_Click(object sender, RoutedEventArgs e)
                {
                    string mdkCode = CodeInput.Text;
                    string mdkCriteria = AnalyzeCode(CodeInput.Text, _task.Number).criteria;
                    string totalScore = AnalyzeCode(CodeInput.Text, _task.Number).totalScore;
                    DateTime now = DateTime.Now;
                    TimeOnly currentTime = TimeOnly.FromDateTime(now);
                    DateOnly currentDate = DateOnly.FromDateTime(now);

                    StudentExamResult studentExamResult = new StudentExamResult
                    {
                        StudentId = _studentId,
                        ExamId = _examId,
                        TimeEnded = currentTime,
                        DateEnded = currentDate,
                        Mdkcode = LOL.Encrypt(mdkCode),
                        Mdkcriteria = LOL.Encrypt(mdkCriteria),
                        TotalScore = LOL.Encrypt(totalScore),
                        TaskNumber = _task.Number
                    };

                    GradeFlowContext context = new GradeFlowContext();
                    context.StudentExamResults.Add(studentExamResult);
                    context.SaveChanges();
                    MessageBox.Show("Результаты отправлены.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }*/

        private void CheckCode_Click(object sender, RoutedEventArgs e)
        {
            string mdkCode = CodeInput.Text;
            string mdkCriteria = AnalyzeCode(mdkCode, _task.Number).criteria;
            string totalScore = AnalyzeCode(mdkCode, _task.Number).totalScore;
            DateTime now = DateTime.Now;
            TimeOnly currentTime = TimeOnly.FromDateTime(now);
            DateOnly currentDate = DateOnly.FromDateTime(now);

            using var context = new GradeFlowContext();

            // Поиск существующего результата по StudentId, ExamId и TaskNumber
            var existingResult = context.StudentExamResults.FirstOrDefault(r =>
                r.StudentId == _studentId &&
                r.ExamId == _examId);

            if (existingResult != null)
            {
                // Обновление существующего результата
                existingResult.TimeEnded = currentTime;
                existingResult.DateEnded = currentDate;
                existingResult.Mdkcode = LOL.Encrypt(mdkCode);
                existingResult.Mdkcriteria = LOL.Encrypt(mdkCriteria);
                existingResult.PracticeTotalScore = LOL.Encrypt(totalScore);
                existingResult.TaskNumber = _task.Number;
            }
            else
            {
                // Добавление нового результата
                StudentExamResult studentExamResult = new StudentExamResult
                {
                    StudentId = _studentId,
                    ExamId = _examId,
                    TimeEnded = currentTime,
                    DateEnded = currentDate,
                    Mdkcode = LOL.Encrypt(mdkCode),
                    Mdkcriteria = LOL.Encrypt(mdkCriteria),
                    PracticeTotalScore = LOL.Encrypt(totalScore),
                    TaskNumber = _task.Number
                };

                context.StudentExamResults.Add(studentExamResult);
            }

            context.SaveChanges();
            MessageBox.Show("Результаты отправлены.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        static class LOL
        {
            private const int KeySize = 32;
            private const int IvSize = 12;
            private const int TagSize = 16;

            public static string Encrypt(string? plainText)
            {
                try
                {
                    if (plainText != null)
                    {
                        var key = "GradeFlowWPF" + ComplexComputation();
                        byte[] keyBytes = GetKey(key);
                        byte[] iv = new byte[IvSize];

                        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                        {
                            rng.GetBytes(iv);
                        }

                        using (AesGcm aes = new AesGcm(keyBytes, TagSize))
                        {
                            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                            byte[] cipherBytes = new byte[plainBytes.Length];
                            byte[] tag = new byte[TagSize];

                            aes.Encrypt(iv, plainBytes, cipherBytes, tag);

                            byte[] encryptedData = new byte[IvSize + cipherBytes.Length + TagSize];
                            Array.Copy(iv, 0, encryptedData, 0, IvSize);
                            Array.Copy(cipherBytes, 0, encryptedData, IvSize, cipherBytes.Length);
                            Array.Copy(tag, 0, encryptedData, IvSize + cipherBytes.Length, TagSize);

                            return Convert.ToBase64String(encryptedData);
                        }
                    }
                    else
                    {
                        return null!;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            public static string Decrypt(string? cipherText)
            {
                try
                {
                    if (cipherText != null)
                    {
                        var key = "GradeFlowWPF" + ComplexComputation();
                        byte[] keyBytes = GetKey(key);

                        cipherText = cipherText.PadRight(cipherText.Length + (4 - cipherText.Length % 4) % 4, '=');
                        byte[] cipherData = Convert.FromBase64String(cipherText);

                        byte[] iv = new byte[IvSize];
                        byte[] tag = new byte[TagSize];
                        byte[] cipherBytes = new byte[cipherData.Length - IvSize - TagSize];

                        Array.Copy(cipherData, 0, iv, 0, IvSize);
                        Array.Copy(cipherData, IvSize, cipherBytes, 0, cipherBytes.Length);
                        Array.Copy(cipherData, IvSize + cipherBytes.Length, tag, 0, TagSize);

                        using (AesGcm aes = new AesGcm(keyBytes, TagSize))
                        {
                            byte[] plainBytes = new byte[cipherBytes.Length];
                            aes.Decrypt(iv, cipherBytes, tag, plainBytes);
                            return Encoding.UTF8.GetString(plainBytes);
                        }
                    }
                    else
                    {
                        return null!;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            private static byte[] GetKey(string key)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                }
            }

            private static string ComplexComputation()
            {
                int[] values = { 1012, 3, 5, 7, 4 };
                int sum = 0;
                for (int i = 0; i < values.Length; i++)
                {
                    sum += values[i] * (i % 2 == 0 ? 2 : 3);
                }
                sum -= Fibonacci(5) * 10;
                sum += Factorial(3);
                return $"{sum}ects2025";
            }

            private static int Fibonacci(int n)
            {
                if (n <= 1) return n;
                return Fibonacci(n - 1) + Fibonacci(n - 2);
            }

            private static int Factorial(int n)
            {
                if (n <= 1) return 1;
                return n * Factorial(n - 1);
            }
        }
    }
}