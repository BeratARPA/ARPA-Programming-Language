namespace ARPA.Logging
{
    public class Logger
    {
        private static readonly string LogFilePath = "log.log";

        public static void Log(string message)
        {
            Console.WriteLine(message); // Konsola yaz
            File.AppendAllText(LogFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}"); // Dosyaya ekle
        }

        public static void LogError(string errorMessage)
        {
            Log($"HATA: {errorMessage}");
        }

        public static void LogInfo(string infoMessage)
        {
            Log($"INFO: {infoMessage}");
        }
    }
}
