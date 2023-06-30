using System.Net.Http;
using System.Threading.Tasks;

namespace Hostel.Infrastructure.Repositories
{
    public interface IWebFilesRepository<TCreateResponse, TDeleteResponse>
       where TCreateResponse : class where TDeleteResponse : class
    {
        
        /// <summary>
        /// Метод загрузки файла
        /// </summary>
        /// <param name="content">Данные из блока памяти</param>
        /// <returns>Ответ от API</returns>
        Task<TCreateResponse> Upload(MultipartFormDataContent content);
        
        /// <summary>
        /// Метод удаления данных
        /// </summary>
        /// <param name="name">Название файла</param>
        /// <returns>Ответ от API</returns>
        Task<TDeleteResponse> Delete(string name);
    }
}
