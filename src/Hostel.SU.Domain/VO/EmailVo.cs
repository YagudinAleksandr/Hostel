using Hostel.Shared.Kernel;
using System.Text.RegularExpressions;

namespace Hostel.SU.Domain
{
    /// <summary>
    /// Адрес электронной почты (Value Object)
    /// </summary>
    public class EmailVo : ValueObject
    {
        /// <summary>
        /// Минимальная длина строки
        /// </summary>
        private const int MinLength = 5;

        /// <summary>
        /// Максимальная длина строки
        /// </summary>
        private const int MaxLength = 60;

        /// <summary>
        /// Регулярное выражение для валидации email (RFC 5322 базовый вариант)
        /// </summary>
        private const string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        /// <param name="value">Значение</param>
        /// <exception cref="DomainValidationFieldException"></exception>
        public EmailVo(string value)
        {
            string val = SetCharField(ServicesUsersFieldCodes.ServicesUsersFieldEmail, value, MinLength, MaxLength);

            if (!Regex.IsMatch(val, EmailPattern))
                throw new DomainValidationFieldException(ServicesUsersFieldCodes.ServicesUsersFieldEmail);

            Value = val;
        }

        /// <summary>
        /// Строковое представление адреса электронной почты
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Value;

        /// <inheritdoc/>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
