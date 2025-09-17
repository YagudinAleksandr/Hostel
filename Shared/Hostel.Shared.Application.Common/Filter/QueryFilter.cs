namespace Hostel.Shared.Application.Common
{
    /// <summary>
    /// Унифицированный фильтр для запросов (поиск, сортировка, пагинация, динамические фильтры)
    /// </summary>
    public class QueryFilter
    {
        /// <summary>
        /// Поисковая строка (общий текстовый поиск)
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// Имя поля для сортировки
        /// </summary>
        public string? SortBy { get; set; }

        /// <summary>
        /// Направление сортировки (asc/desc)
        /// </summary>
        public string? SortDirection { get; set; } = "asc";

        /// <summary>
        /// Номер страницы (начиная с 1)
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Размер страницы
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Динамические фильтры (имя поля -> значение)
        /// </summary>
        public Dictionary<string, string>? Filters { get; set; }
    }
}
