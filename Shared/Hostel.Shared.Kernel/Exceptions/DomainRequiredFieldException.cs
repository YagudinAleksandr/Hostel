namespace Hostel.Shared.Kernel
{
    /// <summary>
    /// Исключение поля обязательного для заполнения
    /// </summary>
    public class DomainRequiredFieldException : DomainException
    {
        /// <summary>
        /// Название поля
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// Исключение поля обязательного для заполнения
        /// </summary>
        /// <param name="fieldName">Название поля</param>
        public DomainRequiredFieldException(string fieldName) : base(DomainExceptionCodes.DomainRequiredFieldException, fieldName)
        {
            FieldName = fieldName;
        }
    }
}
