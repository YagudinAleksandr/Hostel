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
    }
}
