namespace Hostel.Shared.Kernel
{
    /// <summary>
    /// Исключение минимальной длины поля
    /// </summary>
    public class DomainMinLengthFieldException : DomainException
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
        /// Исключение минимальной длины поля
        /// </summary>
        /// <param name="fieldName">Название поля</param>
        /// <param name="length">Длина</param>
        public DomainMinLengthFieldException(string fieldName, int length) 
            : base(DomainExceptionCodes.DomainMinLengthFieldException, fieldName, length)
        {
            FieldName = fieldName;
            Length = length;
        }

        /// <summary>
        /// Исключение минимальной длины поля
        /// </summary>
        /// <param name="fieldName">Название поля</param>
        /// <param name="length">Длина</param>
        public DomainMinLengthFieldException(string fieldName, double length)
            : base(DomainExceptionCodes.DomainMinLengthFieldDigitException, fieldName, length)
        {
            FieldName = fieldName;
            Length = length;
        }
    }
}
