using Hostel.Domain.DTO.FilesDTOs;
using Hostel.Infrastructure.Repositories;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hostel.WebAPIClient
{
    public class WebFilesRepository<TCResponse, TDResponse> : IWebFilesRepository<TCResponse, TDResponse>
        where TCResponse : FileUploadResponseDTO where TDResponse : FileDeleteResponseDTO
    {
        private readonly HttpClient client;
        public WebFilesRepository(HttpClient client)
        {
            this.client = client;
        }

        public async Task<TDResponse> Delete(string name)
        {
            var result = await client.DeleteAsync(name).ConfigureAwait(false);

            return await result.Content.ReadFromJsonAsync<TDResponse>().ConfigureAwait(false);
        }

        public async Task<TCResponse> Upload(MultipartFormDataContent content)
        {
            var result = await client.PostAsync("", content).ConfigureAwait(false);

            return await result.Content.ReadFromJsonAsync<TCResponse>().ConfigureAwait(false);
        }
    }
}
