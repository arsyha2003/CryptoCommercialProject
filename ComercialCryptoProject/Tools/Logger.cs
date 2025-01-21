using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoProject.Tools
{
    public static class Logger
    {
        private static readonly object locker = new object();
        private static readonly string logFilePath = "log.txt";

        /// <summary>
        /// Логирование сообщения в файл.
        /// </summary>
        /// <param name="message">Сообщение для логирования.</param>
        public static void Log(string message)
        {
            lock (locker)
            {
                try
                {
                    using (var writer = new StreamWriter(logFilePath, true))
                    {
                        writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                    }
                }
                catch (Exception ex)
                { }
            }
        }
    }
}
