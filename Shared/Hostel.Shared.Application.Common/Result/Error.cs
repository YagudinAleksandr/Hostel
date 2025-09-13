namespace Hostel.Shared.Application.Common
{
    /// <summary>
    /// Представляет ошибку с кодом и необязательными параметрами для подстановки в сообщение.
    /// Используется в результатах операций вместо исключений.
    /// </summary>
    public record Error(string Code, object[] Parameters = null!)
    {
        /// <summary>
        /// Представляет отсутствие ошибки.
        /// </summary>
        public static readonly Error None = new Error(string.Empty);

        /// <summary>
        /// Указывает, что ошибка отсутствует.
        /// </summary>
        public bool IsNone => this == None;

        /// <summary>
        /// Указывает, что у ошибки есть параметры для подстановки.
        /// </summary>
        public bool HasParameters => Parameters != null && Parameters.Length > 0;
    }
}
