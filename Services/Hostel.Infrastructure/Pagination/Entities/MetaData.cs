namespace Hostel.Infrastructure.Pagination.Entities
{
    /// <summary>
    /// Метаданные возвращаемой страницы
    /// </summary>
    public class MetaData
    {
        // <summary>
        /// Выбранная страница
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Всего страниц
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// Размер страницы
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Всего элементов
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// Отображать ли кнопку назад, если выбранная страница не первая
        /// </summary>
        public bool HasPrevious => CurrentPage > 1;
        /// <summary>
        /// Отображать ли кнопку вперед, если количество меньше чем всего страниц
        /// </summary>
        public bool HasNext => CurrentPage < TotalPages;
    }
}
