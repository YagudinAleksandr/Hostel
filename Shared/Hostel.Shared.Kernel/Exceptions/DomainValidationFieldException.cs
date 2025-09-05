namespace Hostel.Shared.Kernel
{
    /// <summary>
    /// Невалидное поле
    /// </summary>
    public class DomainValidationFieldException : DomainException
    {
        /// <summary>
        /// Название поля
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// Должно содержать
        /// </summary>
        public string? MustHave { get; }

        /// <summary>
        /// Невалидное поле
        /// </summary>
        /// <param name="fieldName">Название поля</param>
        public DomainValidationFieldException(string fieldName) : base(DomainExceptionCodes.DomainValidateFieldException, fieldName)
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// Невалидное поле
        /// </summary>
        /// <param name="fieldName">Название поля</param>
        /// <param name="mustHave">Должно содержать</param>
        public DomainValidationFieldException(string fieldName, string mustHave) :
            base(DomainExceptionCodes.DomainValidateMustHaveFieldException, fieldName, mustHave)
        {
            FieldName = fieldName;
            MustHave = mustHave;
        }
    }
}
