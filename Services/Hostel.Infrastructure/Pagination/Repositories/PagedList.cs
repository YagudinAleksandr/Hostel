using Hostel.Infrastructure.Pagination.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hostel.Infrastructure.Pagination.Repositories
{
    /// <summary>
    /// Класс возвращаемой страницы
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>
    {
        // <summary>
        /// Метаданные страницы
        /// </summary>
        public MetaData MetaData { get; set; }

        /// <summary>
        /// Конструктор получающий данные
        /// </summary>
        /// <param name="items">Список данных</param>
        /// <param name="count">Общее число сущностей</param>
        /// <param name="pageNumber">Страница</param>
        /// <param name="pageSize">Количество элементов на страницу</param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }
        /// <summary>
        /// Переход на страницу
        /// </summary>
        /// <param name="source">Сущности</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Количество элементов на страницу</param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source
              .Skip((pageNumber - 1) * pageSize)
              .Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
