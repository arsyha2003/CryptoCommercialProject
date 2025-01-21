namespace CryptoPtoject.Forms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var exception = args.ExceptionObject as Exception;
                if (exception != null)
                {
                    MessageBox.Show($"Критическая ошибка: {exception.Message}", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            Application.ThreadException += (sender, args) =>
            {
                MessageBox.Show($"Произошла ошибка: {args.Exception.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            Application.Run(new Form1());
        }
    }
}