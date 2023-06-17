namespace Hostel.Infrastructure.Pagination.Entities
{
    /// <summary>
    /// Ссылка на страницу
    /// </summary>
    public class PagingLink
    {
        /// <summary>
        /// Текст ссылки
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Выделена ли
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Активна ли
        /// </summary>
        public bool Active { get; set; }
        public PagingLink(int page, bool enabled, string text)
        {
            Page = page;
            Enabled = enabled;
            Text = text;
        }
    }
}
