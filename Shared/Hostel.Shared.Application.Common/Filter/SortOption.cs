namespace Hostel.Shared.Application.Common
{
    /// <summary>
    /// Опция сортировки для фильтра.
    /// </summary>
    public class SortOption
    {
        /// <summary>
        /// Имя поля для сортировки.
        /// </summary>
        public string Field { get; set; } = string.Empty;

        /// <summary>
        /// Направление сортировки: "asc" или "desc".
        /// </summary>
        public string Direction { get; set; } = "asc";
    }
}
