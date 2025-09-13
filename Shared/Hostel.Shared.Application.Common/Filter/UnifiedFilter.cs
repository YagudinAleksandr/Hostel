namespace Hostel.Shared.Application.Common
{
    /// <summary>
    /// Унифицированный фильтр для получения определенного количества записей с пагинацией, выборкой полей и сортировкой.
    /// </summary>
    public class UnifiedFilter
    {
        /// <summary>
        /// Номер страницы (начиная с 1).
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Количество записей на странице.
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Список полей для выборки (имена свойств DTO).
        /// </summary>
        public List<string> SelectFields { get; set; } = new List<string>();

        /// <summary>
        /// Список опций сортировки.
        /// </summary>
        public List<SortOption> SortOptions { get; set; } = new List<SortOption>();

        /// <summary>
        /// Количество записей, которые нужно пропустить (вычисляется автоматически).
        /// </summary>
        public int Skip => (PageNumber - 1) * PageSize;

        /// <summary>
        /// Количество записей, которые нужно взять (PageSize).
        /// </summary>
        public int Take => PageSize;
    }
}
