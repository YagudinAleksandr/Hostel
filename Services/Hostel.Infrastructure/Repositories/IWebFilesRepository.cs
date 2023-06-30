using System.Net.Http;
using System.Threading.Tasks;

namespace Hostel.Infrastructure.Repositories
{
    public interface IWebFilesRepository<TCreateResponse, TDeleteResponse>
       where TCreateResponse : class where TDeleteResponse : class
    {
        /// <summary>
        /// Загрузка файлов
        /// </summary>
        /// <param name="request">Тело запроса на создание</param>
        /// <returns>Ответ от API</returns>
        Task<TCreateResponse> Upload(MultipartFormDataContent content);
        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="request">Тело запроса</param>
        /// <returns>Ответ от API</returns>
        Task<TDeleteResponse> Delete(string name);
    }
}
