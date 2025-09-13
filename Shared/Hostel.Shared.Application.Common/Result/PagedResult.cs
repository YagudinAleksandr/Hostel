namespace Hostel.Shared.Application.Common
{
    /// <summary>
    /// Представляет результат постраничного запроса, содержащий коллекцию элементов и метаданные пагинации.
    /// </summary>
    /// <typeparam name="T">Тип элементов в коллекции.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PagedResult{T}"/> с указанными данными.
        /// </summary>
        /// <param name="items">Коллекция элементов на текущей странице.</param>
        /// <param name="totalCount">Общее количество элементов во всех страницах.</param>
        /// <param name="pageNumber">Номер текущей страницы (начинается с 1).</param>
        /// <param name="pageSize">Размер одной страницы (количество элементов на странице).</param>
        public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            TotalCount = totalCount;
            PageNumber = pageNumber > 0 ? pageNumber : throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть больше 0.");
            PageSize = pageSize > 0 ? pageSize : throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть больше 0.");
        }

        /// <summary>
        /// Коллекция элементов на текущей странице.
        /// </summary>
        public IEnumerable<T> Items { get; }

        /// <summary>
        /// Общее количество элементов во всех страницах.
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// Номер текущей страницы (начинается с 1).
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// Размер одной страницы (количество элементов на странице).
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Общее количество страниц, рассчитанное на основе общего количества элементов и размера страницы.
        /// </summary>
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

        /// <summary>
        /// Указывает, есть ли страница до текущей (т.е. можно ли перейти на предыдущую страницу).
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Указывает, есть ли страница после текущей (т.е. можно ли перейти на следующую страницу).
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>
        /// Создаёт пустой результат пагинации с указанными параметрами.
        /// </summary>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <param name="pageNumber">Номер текущей страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Экземпляр <see cref="PagedResult{T}"/> с пустой коллекцией элементов.</returns>
        public static PagedResult<T> Empty(int totalCount, int pageNumber, int pageSize)
        {
            return new PagedResult<T>(Enumerable.Empty<T>(), totalCount, pageNumber, pageSize);
        }

        /// <summary>
        /// Создаёт экземпляр <see cref="PagedResult{T}"/> из коллекции, применяя пагинацию.
        /// </summary>
        /// <param name="source">Исходная коллекция элементов.</param>
        /// <param name="pageNumber">Номер страницы (начиная с 1).</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Экземпляр <see cref="PagedResult{T}"/> с элементами текущей страницы и метаданными.</returns>
        public static PagedResult<T> From(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Номер страницы должен быть >= 1.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Размер страницы должен быть >= 1.");

            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var totalCount = source is ICollection<T> collection ? collection.Count : source.Count();

            return new PagedResult<T>(items, totalCount, pageNumber, pageSize);
        }
    }
}
