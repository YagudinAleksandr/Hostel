using System.Threading.Tasks;

namespace Hostel.Infrastructure.Repositories
{
    public interface IWebFilesRepository<TCreateRequest, TCreateResponse, TDeleteRequest, TDeleteResponse>
        where TCreateRequest : class where TCreateResponse : class where TDeleteRequest : class where TDeleteResponse : class
    {
        /// <summary>
        /// Загрузка файлов
        /// </summary>
        /// <param name="request">Тело запроса на создание</param>
        /// <returns>Ответ от API</returns>
        Task<TCreateResponse> Upload(TCreateRequest request);
        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="request">Тело запроса</param>
        /// <returns>Ответ от API</returns>
        Task<TDeleteResponse> Delete(TDeleteRequest request);
    }
}
