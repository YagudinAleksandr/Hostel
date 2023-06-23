using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Hostel.BlazorUI.Infrastructure.Extensions
{
    /// <summary>
    /// Класс расширения для подключения сервисов HTTP Client
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Добавление сервисов работы через API
        /// </summary>
        /// <typeparam name="IInterface">Интерфейс репозитория</typeparam>
        /// <typeparam name="IClient">Клиент реализовывающий интерфейс</typeparam>
        /// <param name="services">Сервис HTTP</param>
        /// <param name="address">Адрес обращения к API</param>
        /// <returns></returns>
        public static IHttpClientBuilder AddApi<IInterface, IClient>(this IServiceCollection services, string address)
            where IInterface : class where IClient : class, IInterface => services
            .AddHttpClient<IInterface, IClient>(
                (host, client) => client.BaseAddress = new($"{host.GetRequiredService<IWebAssemblyHostEnvironment>().BaseAddress}{address}"));
    }
}
