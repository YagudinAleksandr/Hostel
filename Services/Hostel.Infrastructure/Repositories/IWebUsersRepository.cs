using Hostel.Infrastructure.Pagination.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Hostel.Infrastructure.Repositories
{
    public interface IWebUsersRepository<TCreateDTO, TUpdateDTO, TResponseDTO> where TCreateDTO : class where TUpdateDTO : class where TResponseDTO : class
    {
        /// <summary>
        /// Добавление сущности
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="cancel">Отмена</param>
        /// <returns>Добавленная сущность</returns>
        Task<TResponseDTO> Add(TCreateDTO entity, CancellationToken cancel = default);
        /// <summary>
        /// Обновление сущности
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="cancel">Отмена</param>
        /// <returns>Обновленная сущность</returns>
        Task<TResponseDTO> Update(TUpdateDTO entity, CancellationToken cancel = default);
        /// <summary>
        /// Удаление сущности
        /// </summary>
        /// <param name="id">ID сущности</param>
        /// <param name="cancel">Отмена</param>
        /// <returns>True - сущность удалена, False - возникла ошибка при удалении</returns>
        Task<TResponseDTO> Delete(string id, CancellationToken cancel = default);
        /// <summary>
        /// Получение сущности
        /// </summary>
        /// <param name="id">ID сущности</param>
        /// <param name="cancel">Отмена</param>
        /// <returns>Сущность</returns>
        Task<TResponseDTO> Get(string id, CancellationToken cancel = default);
        /// <summary>
        /// Возвращение всех сущностей
        /// </summary>
        /// <param name="usersParameters">Параметры страницы</param>
        /// <param name="cancel">Отмена</param>
        /// <returns>Ответ страницы</returns>
        Task<PagingResponse<TResponseDTO>> GetAll(PageParametrs usersParameters, CancellationToken cancel = default);
    }
}
