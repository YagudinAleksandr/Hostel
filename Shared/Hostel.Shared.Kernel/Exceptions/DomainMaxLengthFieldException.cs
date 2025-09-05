namespace Hostel.Shared.Kernel
{
    /// <summary>
    /// Исключение максимальной длины поля
    /// </summary>
    public class DomainMaxLengthFieldException : DomainException
    {
        /// <summary>
        /// Название поля
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// Длина
        /// </summary>
        public double Length { get; }

        /// <summary>
        /// Исключение максимальной длины поля
        /// </summary>
        /// <param name="fieldName">Название поля</param>
        /// <param name="length">Длина</param>
        public DomainMaxLengthFieldException(string fieldName, int length) 
            : base(DomainExceptionCodes.DomainMaxLengthFieldException, fieldName, length)
        {
            FieldName = fieldName;
            Length = length;
        }

        /// <summary>
        /// Исключение максимальной длины поля
        /// </summary>
        /// <param name="fieldName">Название поля</param>
        /// <param name="length">Длина</param>
        public DomainMaxLengthFieldException(string fieldName, double length)
            : base(DomainExceptionCodes.DomainMaxLengthFieldDigitException, fieldName, length)
        {
            FieldName = fieldName;
            Length = length;
        }
    }
}
