namespace Hostel.Shared.Kernel
{
    /// <summary>
    /// Исключение значения не попадающего в диапозон
    /// </summary>
    public class DomainRangeFieldException : DomainException
    {
        /// <summary>
        /// Название поля
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// Минимальное значение
        /// </summary>
        public double MinValue { get; }

        /// <summary>
        /// Максимальное значение
        /// </summary>
        public double MaxValue { get; }

        /// <summary>
        /// Исключение значения не попадающего в диапозон
        /// </summary>
        /// <param name="fieldName">Название поля</param>
        /// <param name="minValue">Минимальное значение</param>
        /// <param name="maxValue">Максимальное значение</param>
        public DomainRangeFieldException(string fieldName, double minValue, double maxValue) 
            : base(DomainExceptionCodes.DomainRangeFieldException, fieldName, minValue, maxValue)
        {
            FieldName = fieldName;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
