using Hostel.BlazorUI.Infrastructure.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hostel.Infrastructure.Repositories;
using Hostel.Domain.DTO.UsersDTOs;
using Hostel.WebAPIClient;

namespace Hostel.BlazorUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var service = builder.Services;

            service.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });

            #region Подключение сервисов

            service.AddApi<IWebUsersRepository<UserCreateRequestDTO, UserUpdateDTO, UserResponseDTO>, WebUsersRepository<UserCreateRequestDTO, UserUpdateDTO, UserResponseDTO>>("https://localhost:5011/api", "Users/");

            #endregion

            await builder.Build().RunAsync();
        }
    }
}
