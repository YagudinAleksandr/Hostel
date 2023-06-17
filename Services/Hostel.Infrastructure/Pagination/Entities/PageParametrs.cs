namespace Hostel.Infrastructure.Pagination.Entities
{
    /// <summary>
    /// Класс параметров страницы
    /// </summary>
    public class PageParametrs
    {
        /// <summary>
        /// Максимальное число элементов на странице
        /// </summary>
        const int maxPageItems = 50;

        /// <summary>
        /// Номер страницы
        /// </summary>
        public int PageNumber { get; set; } = 1;
        /// <summary>
        /// Поле размера страницы
        /// </summary>
        private int pageSize = 10;

        /// <summary>
        /// Заданный размер страницы
        /// </summary>
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = (value > maxPageItems) ? maxPageItems : value;
            }
        }
    }
}
