using System.Security.Cryptography;
using System.Text;
using System.Windows;
using GradeFlowECTS.Analyzers.MDK01;
using GradeFlowECTS.Infrastructure;
using GradeFlowECTS.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GradeFlowECTS.View.Windows
{
    public partial class StudentMDK01Window : Window
    {
        private List<(byte Number, string TaskText)> _tasks;
        private (byte Number, string TaskText) _task;
        private int _studentId;
        private Guid _examId;

        public StudentMDK01Window(int studentId, Guid examId)
        {
            InitializeComponent();

            _studentId = studentId;
            _examId = examId;

            _tasks = new()
            {
                (1, "Запросить ввод N - размер массива. Заполнить массив размера N с клавиатуры. Вывести все содержащиеся в данном массиве нечетные числа в порядке возрастания их индексов, а также их количество K."),
                (2, "Запросить ввод N - размер массива. Заполнить массив размера N с клавиатуры. Вывести вначале все содержащиеся в данном массиве четные числа в порядке возрастания их индексов, а затем — все нечетные числа в порядке убывания их индексов."),
                (3, "Запросить ввод N - размер массива. Заполнить массив размера N с клавиатуры. Ввести с клавиатуры целые числа K и L (1 < K ≤ L ≤ N). Найти сумму всех элементов массива, кроме элементов с номерами от K до L включительно."),
                (4, "Запросить ввод N - размер массива. Заполнить массив размера N с клавиатуры. Ввести с клавиатуры целые числа K и L (1 < K ≤ L ≤ N). Найти среднее арифметическое всех элементов массива, кроме элементов с номерами от K до L включительно."),
                (5, "Запросить ввод N - размер массива. Заполнить массив размера N с клавиатуры. Найти номера тех элементов массива, которые больше своего правого соседа, и количество таких элементов. Найденные номера выводить в порядке их возрастания. Нельзя использовать существующие методы сортировки."),
                (6, "Запросить ввод N - размер массива. Заполнить массив размера N с клавиатуры. Найти номера тех элементов массива, которые больше своего левого соседа, и количество таких элементов. Найденные номера выводить в порядке их убывания. Нельзя использовать существующие методы сортировки."),
                (7, "Запросить ввод N - размер массива. Заполнить массив размера N с клавиатуры. Упорядочить его по возрастанию методом сортировки простым обменом («пузырьковой» сортировкой): просматривать массив, сравнивая его соседние элементы (A1 и A2, A2 и A3 и т. д.) и меняя их местами, если левый элемент пары больше правого; повторить описанные действия N − 1 раз. Для контроля за выполняемыми действиями выводить содержимое массива после каждого просмотра. Учесть, что при каждом просмотре количество анализируемых пар можно уменьшить на 1."),

                (8, "Запросить ввод М и N - размеры матрицы. Запросить ввод, где K - целое число (1 ≤ K ≤ M). Заполнить матрицу с клавиатуры. Вывести полученные значения матрицы. Вывести элементы K-й строки данной матрицы."),
                (9, "Запросить ввод М и N - размеры матрицы. Запросить ввод, где K - целое число (1 ≤ K ≤ N). Заполнить матрицу с клавиатуры. Вывести полученные значения матрицы. Вывести элементы K-го столбца данной матрицы."),

                (10, "Запросить ввод N - размер массива. Заполнить массив размера N с клавиатуры. Вывести все чётные числа в порядке возрастания их индексов, а затем все нечётные числа в порядке убывания их индексов."),
                (11, "Запросить ввод N – размер массива. Заполнить массив размера N с клавиатуры. Найти и вывести минимальный и максимальный элементы массива, а также их индексы (если таких элементов несколько, вывести первые встретившиеся)."),
                (12, "Запросить ввод N – размер массива. Заполнить массив размера N с клавиатуры. Ввести с клавиатуры целые числа K и L (1 ≤ K ≤ L ≤ N). Найти сумму всех элементов массива с номерами от K до L включительно."),
                (13, "Запросить ввод N – размер массива. Заполнить массив размера N с клавиатуры. Ввести число X. Вывести количество элементов массива, кратных X, и их значения."),
                (14, "Запросить ввод N – размер массива. Заполнить массив размера N с клавиатуры. Заменить все четные элементы массива на нули и вывести измененный массив."),
                (15, "Запросить ввод N – размер массива. Заполнить массив размера N с клавиатуры. Найти среднее арифметическое всех положительных элементов массива."),
                (16, "Запросить ввод N – размер массива. Заполнить массив размера N с клавиатуры. Сформировать новый массив, исключив из исходного все элементы, кратные 3, и вывести его."),
                (17, "Запросить ввод N – размер массива. Заполнить массив размера N с клавиатуры. Найти два наименьших элемента массива и вывести их значения и индексы."),

                (18, "Запросить ввод M и N – размеры матрицы. Заполнить матрицу с клавиатуры. Вывести полученные значения матрицы. Ввести число K (1 ≤ K ≤ M). Найти и вывести сумму элементов K-й строки."),
                (19, "Запросить ввод M и N – размеры матрицы. Заполнить матрицу с клавиатуры. Вывести полученные значения матрицы. Ввести число K (1 ≤ K ≤ N). Найти и вывести среднее арифметическое элементов K-го столбца."),
                (20, "Запросить ввод M и N – размеры матрицы. Заполнить матрицу с клавиатуры. Вывести полученные значения матрицы. Найти строку с наибольшей суммой элементов и вывести её номер и сумму."),
                (21, "Запросить ввод M и N – размеры матрицы. Заполнить матрицу с клавиатуры. Вывести полученные значения матрицы. Транспонировать матрицу (строки <-> столбцы) и вывести результат."),
                (22, "Запросить ввод M и N – размеры матрицы. Заполнить матрицу с клавиатуры. Вывести полученные значения матрицы. Ввести числа K и L (1 ≤ K, L ≤ M). Поменять местами строки K и L, затем вывести измененную матрицу."),
                (23, "Запросить ввод M и N – размеры матрицы. Заполнить матрицу с клавиатуры. Вывести полученные значения матрицы. Ввести числа K и L (1 ≤ K, L ≤ N). Поменять местами столбцы K и L, затем вывести измененную матрицу."),
                (24, "Запросить ввод M и N – размеры матрицы. Заполнить матрицу с клавиатуры. Вывести полученные значения матрицы. Для каждого столбца найти и вывести максимальный элемент и его индекс строки."),
                (25, "Запросить ввод M и N – размеры матрицы. Заполнить матрицу с клавиатуры. Вывести полученные значения матрицы. Найти строку с максимальной суммой и строку с минимальной суммой."),
                (26, "Запросить ввод M и N – размеры матрицы. Заполнить матрицу с клавиатуры. Вывести полученные значения матрицы. Для каждой строки вывести количество положительных элементов."),
                (27, "Запросить ввод M и N – размеры матрицы. Заполнить матрицу с клавиатуры. Вывести полученные значения матрицы. Найти элемент, который является минимальным в своей строке. Вывести его значение и индексы."),
                (28, "Запросить ввод M и N – размеры матрицы. Заполнить матрицу с клавиатуры. Вывести полученные значения матрицы. Найти элемент, который является максимальным в своем столбце. Вывести его значение и индексы."),
                (29, "Запросить ввод M и N – размеры матрицы. Заполнить матрицу с клавиатуры. Вывести полученные значения матрицы. Поменять местами первую и последнюю строки матрицы и вывести результат."),
                (30, "Запросить ввод M и N – размеры матрицы. Заполнить матрицу с клавиатуры. Вывести полученные значения матрицы. Поменять местами первый и последний столбец матрицы и вывести результат."),
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
                        return (Task1Analyzer.Analyze(root).totalScore, Task1Analyzer.Analyze(root).criteria);
                    case 2:
                        return (Task2Analyzer.Analyze(root).totalScore, Task2Analyzer.Analyze(root).criteria);
                    case 3:
                        return (Task3Analyzer.Analyze(root).totalScore, Task3Analyzer.Analyze(root).criteria);
                    case 4:
                        return (Task4Analyzer.Analyze(root).totalScore, Task4Analyzer.Analyze(root).criteria);
                    case 5:
                        return (Task5Analyzer.Analyze(code).totalScore, Task5Analyzer.Analyze(code).criteria);
                    case 6:
                        return (Task6Analyzer.Analyze(code).totalScore, Task6Analyzer.Analyze(code).criteria);
                    case 7:
                        return (Task7Analyzer.Analyze(code).totalScore, Task7Analyzer.Analyze(code).criteria);
                    case 8:
                        return (Task8Analyzer.Analyze(root).totalScore, Task8Analyzer.Analyze(root).criteria);
                    case 9:
                        return (Task9Analyzer.Analyze(root).totalScore, Task9Analyzer.Analyze(root).criteria);
                    case 10:
                        return (Task10Analyzer.Analyze(root).totalScore, Task10Analyzer.Analyze(root).criteria);
                    case 11:
                        return (Task11Analyzer.Analyze(root).totalScore, Task11Analyzer.Analyze(root).criteria);
                    case 12:
                        return (Task12Analyzer.Analyze(root).totalScore, Task12Analyzer.Analyze(root).criteria);
                    case 13:
                        return (Task13Analyzer.Analyze(root).totalScore, Task13Analyzer.Analyze(root).criteria);
                    case 14:
                        return (Task14Analyzer.Analyze(root).totalScore, Task14Analyzer.Analyze(root).criteria);
                    case 15:
                        return (Task15Analyzer.Analyze(root).totalScore, Task15Analyzer.Analyze(root).criteria);
                    case 16:
                        return (Task16Analyzer.Analyze(root).totalScore, Task16Analyzer.Analyze(root).criteria);
                    case 17:
                        return (Task17Analyzer.Analyze(root).totalScore, Task17Analyzer.Analyze(root).criteria);
                    case 18:
                        return (Task18Analyzer.Analyze(root).totalScore, Task18Analyzer.Analyze(root).criteria);
                    case 19:
                        return (Task19Analyzer.Analyze(root).totalScore, Task19Analyzer.Analyze(root).criteria);
                    case 20:
                        return (Task20Analyzer.Analyze(root).totalScore, Task20Analyzer.Analyze(root).criteria);
                    case 21:
                        return (Task21Analyzer.Analyze(root).totalScore, Task21Analyzer.Analyze(root).criteria);
                    case 22:
                        return (Task22Analyzer.Analyze(root).totalScore, Task22Analyzer.Analyze(root).criteria);
                    case 23:
                        return (Task23Analyzer.Analyze(root).totalScore, Task23Analyzer.Analyze(root).criteria);
                    case 24:
                        return (Task24Analyzer.Analyze(root).totalScore, Task24Analyzer.Analyze(root).criteria);
                    case 25:
                        return (Task25Analyzer.Analyze(root).totalScore, Task25Analyzer.Analyze(root).criteria);
                    case 26:
                        return (Task26Analyzer.Analyze(root).totalScore, Task26Analyzer.Analyze(root).criteria);
                    case 27:
                        return (Task27Analyzer.Analyze(root).totalScore, Task27Analyzer.Analyze(root).criteria);
                    case 28:
                        return (Task28Analyzer.Analyze(root).totalScore, Task28Analyzer.Analyze(root).criteria);
                    case 29:
                        return (Task29Analyzer.Analyze(root).totalScore, Task29Analyzer.Analyze(root).criteria);
                    case 30:
                        return (Task30Analyzer.Analyze(root).totalScore, Task30Analyzer.Analyze(root).criteria);
                    default:
                        return ("0/0", "Неверный номер задания.");
                }
            }
            catch (Exception ex)
            {
                return ("0/0", "Ошибка анализа: " + ex.Message);
            }
        }

        private void CheckCode_Click(object sender, RoutedEventArgs e)
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