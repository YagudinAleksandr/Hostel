using Hostel.Shared.Kernel;

namespace Hostel.Domain.Primitives
{
    /// <summary>
    /// ФИО (Value Object)
    /// </summary>
    public class FullNameVo : ValueObject
    {
        /// <summary>
        /// Минимальная длина строки
        /// </summary>
        private const int MinLength = 1;

        /// <summary>
        /// Макисмальная длина строки
        /// </summary>
        private const int MaxLength = 30;

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string? Patronymic { get; }

        /// <summary>
        /// ФИО
        /// </summary>
        /// <param name="firstName">Имя</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="patronimic">Отчество</param>
        public FullNameVo(string firstName, string lastName, string? patronimic)
        {
            FirstName = SetCharField(PrimitivesFieldCodes.PrimitiveFieldFirstName, firstName, MinLength, MaxLength);
            LastName = SetCharField(PrimitivesFieldCodes.PrimitiveFieldLastName, lastName, MinLength, MaxLength);

            if (!string.IsNullOrEmpty(patronimic))
                Patronymic = SetCharField(PrimitivesFieldCodes.PrimitiveFieldPatronymic, patronimic, MinLength, MaxLength);
        }

        /// <summary>
        /// Строковое представление ФИО
        /// </summary>
        /// <returns>Строка ФИО</returns>
        public override string ToString()
        {
            string fullName = $"{LastName} {FirstName}";
            fullName += !string.IsNullOrEmpty(Patronymic) ? $" {Patronymic}" : string.Empty;

            return fullName;
        }

        /// <summary>
        /// Сокращенное представление ФИО
        /// </summary>
        /// <returns>Строка формата Иванов И.И.</returns>
        public string ShortVariationOfName()
        {
            string fullShortName = $"{LastName} {FirstName[0]}.";

            fullShortName += !string.IsNullOrEmpty(Patronymic) ? $" {Patronymic[0]}." : string.Empty;

            return fullShortName;
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LastName;
            yield return FirstName;
            yield return Patronymic ?? string.Empty;
        }
    }
}
