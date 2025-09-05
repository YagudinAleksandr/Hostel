namespace Hostel.Shared.Kernel
{
    /// <summary>
    /// Доменное исключение
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Код ошибки для локализации
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// Параметры для подстановки в сообщение
        /// </summary>
        public object[] Parameters { get; }

        /// <summary>
        /// Создать исключение с кодом ошибки и параметрами.
        /// </summary>
        public DomainException(string errorCode, params object[] parameters)
            : base($"Domain error occurred: {errorCode}")
        {
            ErrorCode = errorCode;
            Parameters = parameters;
        }

        /// <summary>
        /// Создать исключение с кодом ошибки, параметрами и внутренним исключением.
        /// </summary>
        public DomainException(string errorCode, Exception innerException, params object[] parameters)
            : base($"Domain error occurred: {errorCode}", innerException)
        {
            ErrorCode = errorCode;
            Parameters = parameters;
        }
    }
}
