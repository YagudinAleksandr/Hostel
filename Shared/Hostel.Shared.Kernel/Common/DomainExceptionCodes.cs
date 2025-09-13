namespace Hostel.Shared.Kernel
{
    /// <summary>
    /// Коды доменных исключений
    /// </summary>
    public static class DomainExceptionCodes
    {
        /// <summary>
        /// Обязательное для заполнения поле
        /// </summary>
        public const string DomainRequiredFieldException = "domain.exception.field.required";

        /// <summary>
        /// Минимальная длина поля
        /// </summary>
        public const string DomainMinLengthFieldException = "domain.exception.field.min_length";

        /// <summary>
        /// Максимальная длина поля
        /// </summary>
        public const string DomainMaxLengthFieldException = "domain.exception.field.max_length";

        /// <summary>
        /// Минимальная длина поля (числовое)
        /// </summary>
        public const string DomainMinLengthFieldDigitException = "domain.exception.field.min_length.digit";

        /// <summary>
        /// Максимальная длина поля (числовое)
        /// </summary>
        public const string DomainMaxLengthFieldDigitException = "domain.exception.field.max_length.digit";

        /// <summary>
        /// Диапозон значений
        /// </summary>
        public const string DomainRangeFieldException = "domain.exception.field.range";

        /// <summary>
        /// Не валидное поле
        /// </summary>
        public const string DomainValidateFieldException = "domain.exception.field.validation";

        /// <summary>
        /// Не валидное поле должно содержать
        /// </summary>
        public const string DomainValidateMustHaveFieldException = "domai.exception.field.validation.must_have";

        /// <summary>
        /// Ресурс не найден
        /// </summary>
        public const string DomainResourceNotFoundException = "domain.exception.resource_not_found";

        /// <summary>
        /// Ресурс не найден с параметром
        /// </summary>
        public const string DomainResourceNotFoundWithParam = "domain.exception.resource_not_found_param";

        /// <summary>
        /// Ресурс уже существует с параметрами
        /// </summary>
        public const string DomainResourceAlreadyExistsParam = "domain.exception.resource_already_exists_param";

        /// <summary>
        /// Ресурс уже существует
        /// </summary>
        public const string DomainResourceAlreadyExists = "domain.exception.resource_already_exists";

        /// <summary>
        /// Ошибка сервера
        /// </summary>
        public const string InternalServerError = "internal_server_error";
    }
}
