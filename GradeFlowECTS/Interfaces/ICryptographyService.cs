namespace GradeFlowECTS.Interfaces
{
    /// <summary>
    /// Сервис для выполнения операций шифрования и расшифровки строковых данных.
    /// </summary>
    public interface ICryptographyService
    {
        /// <summary>
        /// Расшифровывает заданный зашифрованный текст с использованием указанного ключа.
        /// </summary>
        /// <param name="cipherText">Зашифрованный текст (может быть null).</param>
        /// <param name="key">Ключ для расшифровки.</param>
        /// <returns>Расшифрованный текст.</returns>
        string Decrypt(string? cipherText, string key);

        /// <summary>
        /// Шифрует заданный текст с использованием указанного ключа.
        /// </summary>
        /// <param name="plainText">Обычный текст для шифрования (может быть null).</param>
        /// <param name="key">Ключ для шифрования.</param>
        /// <returns>Зашифрованный текст.</returns>
        string Encrypt(string? plainText, string key);
    }
}