namespace Hostel.Shared.Kernel
{
    /// <summary>
    /// Ресурс не найден
    /// </summary>
    public class DomainResourceNotFoundException : DomainException
    {
        /// <summary>
        /// Название ресурса
        /// </summary>
        public string ResourceName { get; }

        /// <summary>
        /// Название параметра/поля
        /// </summary>
        public string? ParamName { get; }

        /// <summary>
        /// Значение параметра
        /// </summary>
        public object? ParamValue { get; }

        /// <summary>
        /// Ресурс не найден
        /// </summary>
        /// <param name="resourceName">Название ресурса</param>
        public DomainResourceNotFoundException(string resourceName)
            : base(DomainExceptionCodes.DomainResourceNotFoundException, resourceName)
        {
            ResourceName = resourceName;
        }

        /// <summary>
        /// Ресурс не найден
        /// </summary>
        /// <param name="resourceName">Название ресурса</param>
        /// <param name="paramName">Название параметра</param>
        /// <param name="paramValue">Значение параметра</param>
        public DomainResourceNotFoundException(string resourceName, string paramName, object paramValue) 
            : base(DomainExceptionCodes.DomainResourceNotFoundWithParam, resourceName, paramName, paramValue)
        {
            ParamName = paramName;
            ParamValue = paramValue;
            ResourceName = resourceName;
        }
    }
}
