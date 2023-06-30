using Hostel.Domain.DTO.UsersDTOs;
using Hostel.Infrastructure.Pagination.Entities;
using Hostel.Infrastructure.Repositories;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Hostel.WebAPIClient
{
    public class WebUsersRepository<TCreate, TUpdate, TResponse> : IWebUsersRepository<TCreate, TUpdate, TResponse>
        where TCreate : UserCreateRequestDTO where TUpdate : UserUpdateDTO where TResponse : UserResponseDTO
    {
        #region Поля
        private readonly HttpClient client;
        private readonly JsonSerializerOptions options;
        #endregion

        public WebUsersRepository(HttpClient client)
        {
            this.client = client;

            this.options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<TResponse> Add(TCreate entity, CancellationToken cancel = default)
        {
            var content = JsonSerializer.Serialize(entity);

            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var addResult = await client.PostAsync("", bodyContent, cancel).ConfigureAwait(false);

            return await addResult.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
        }

        public async Task<TResponse> Delete(string id, CancellationToken cancel = default)
        {
            var response = await client.DeleteAsync($"{id}", cancel).ConfigureAwait(false);

            return await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false);
        }

        public async Task<TResponse> Get(string id, CancellationToken cancel = default) =>
            await client.GetFromJsonAsync<TResponse>($"{id}", cancel).ConfigureAwait(false);

        public async Task<PagingResponse<TResponse>> GetAll(PageParametrs usersParameters, CancellationToken cancel = default)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = usersParameters.PageNumber.ToString()
            };

            var response = await client.GetAsync(QueryHelpers.AddQueryString("", queryStringParam), cancel).ConfigureAwait(false);

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var pagingResponse = new PagingResponse<TResponse>
            {
                Items = JsonSerializer.Deserialize<List<TResponse>>(content, options),
                MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), options)
            };

            return pagingResponse; ;
        }

        public async Task<TResponse> Update(TUpdate entity, CancellationToken cancel = default)
        {
            var response = await client.PutAsJsonAsync("", entity, cancel).ConfigureAwait(false);

            var result = await response.Content
               .ReadFromJsonAsync<TResponse>(cancellationToken: cancel)
               .ConfigureAwait(false);

            return result;
        }
    }
}
