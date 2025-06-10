using System.Security.Cryptography;
using System.Text;
using GradeFlowECTS.Interfaces;

namespace GradeFlowECTS.Infrastructure
{
    public class AesGcmCryptographyService : ICryptographyService
    {
        private const int KeySize = 32;
        private const int IvSize = 12;
        private const int TagSize = 16;

        public string Encrypt(string? plainText, string key)
        {
            try
            {
                if (plainText != null)
                {
                    key += ComplexComputation();
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

        public string Decrypt(string? cipherText, string key)
        {
            try
            {
                if (cipherText != null)
                {
                    key += ComplexComputation();
                    Console.WriteLine(key);
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