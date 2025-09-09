namespace Hostel.Shared.Kernel
{
    /// <summary>
    /// Исключение о существующем ресурсе
    /// </summary>
    public class DomainResourceAlreadyExistsException : DomainException
    {
        /// <summary>
        /// Код сущности
        /// </summary>
        public string EntityCode { get; }

        /// <summary>
        /// Название параметра
        /// </summary>
        public string? EntityParamName { get; }

        /// <summary>
        /// Значение параметра
        /// </summary>
        public string? EntityParamValue { get; }

        /// <summary>
        /// Исключение о существующем ресурсе
        /// </summary>
        /// <param name="entityCode">Код сущности</param>
        public DomainResourceAlreadyExistsException(string entityCode)
            : base(DomainExceptionCodes.DomainResourceAlreadyExists)
        {
            EntityCode = entityCode;
        }

        /// <summary>
        /// Исключение о существующем ресурсе
        /// </summary>
        /// <param name="entityCode">Код сущности</param>
        /// <param name="paramName">Код поля</param>
        /// <param name="paramValue">Значение параметра</param>
        public DomainResourceAlreadyExistsException(string entityCode, string paramName, string paramValue)
            : base(DomainExceptionCodes.DomainResourceAlreadyExistsParam, entityCode, paramName, paramValue)
        {
            EntityCode = entityCode;
            EntityParamName = paramName;
            EntityParamValue = paramValue;
        }
    }
}
