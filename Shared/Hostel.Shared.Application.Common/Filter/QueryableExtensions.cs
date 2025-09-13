using System.Linq.Dynamic.Core;

namespace Hostel.Shared.Application.Common
{
    /// <summary>
    /// Расширение для сортировки запроса <see cref="IQueryable"/>
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Метод сортировки по параметрам
        /// </summary>
        /// <typeparam name="T">Тип данных</typeparam>
        /// <param name="source">Источник</param>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="ascending">По убыванию</param>
        /// <returns>Отсортированный список</returns>
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string propertyName, bool ascending)
        {
            var param = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");
            var property = System.Linq.Expressions.Expression.Property(param, propertyName);
            var lambda = System.Linq.Expressions.Expression.Lambda(property, param);

            string methodName = ascending ? "OrderBy" : "OrderByDescending";
            var result = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type)
                .Invoke(null, new object[] { source, lambda });

            return (IQueryable<T>)result!;
        }

        /// <summary>
        /// Проекция применения полей
        /// </summary>
        /// <typeparam name="T">Тип данных DTO</typeparam>
        /// <param name="propertiesToSelect">Свойства для сортировки</param>
        /// <param name="source">Источник</param>
        /// <returns>Отсортированная коллекция</returns>
        public static IQueryable<dynamic> ApplyDynamicProjection<T>(
            this IQueryable<T> source,
            List<string> propertiesToSelect)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (propertiesToSelect == null || !propertiesToSelect.Any())
            {
                // Если поля не указаны, возвращаем все поля как dynamic
                return source.Select(x => (dynamic)x);
            }

            // Валидация имен полей (важно для безопасности)
            var validProperties = typeof(T).GetProperties()
                .Select(p => p.Name)
                .ToArray();

            var invalidProperties = propertiesToSelect
                .Except(validProperties, StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (invalidProperties.Any())
            {
                throw new ArgumentException($"Invalid properties: {string.Join(", ", invalidProperties)}");
            }

            // Создаем строку для проекции
            var selectString = $"new ({string.Join(", ", propertiesToSelect)})";

            // Используем Select из Dynamic LINQ и явно приводим к IQueryable<dynamic>
            return source.Select(selectString).Cast<dynamic>();
        }
    }
}
