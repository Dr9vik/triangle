namespace Triangle.ConfiguringApps
{
    public class ExceptionMiddleware
    {
        public delegate void ExceptionHandler(string message);
        public event ExceptionHandler? Notify;
        public ExceptionMiddleware()
        {
            Application.ThreadException += ApplicationThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        }
        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var message = String.Format(((Exception)e.ExceptionObject).Message, ((Exception)e.ExceptionObject).StackTrace);
            Notify?.Invoke(message);
        }

        private void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var message = String.Format(e.Exception.Message, e.Exception.StackTrace);
            Notify?.Invoke(message);
        }
    }
}
