using FluentValidation;
using Microsoft.Extensions.Options;
using TSWebAPI.Common.Options;
using TSWebAPI.Dtos;
using TSWebAPI.Dtos.Validators;
using TSWebAPI.Infrastructure;
using TSWebAPI.Interfaces;
using TSWebAPI.Services;

namespace TSWebAPI.Extensions
{
    public static class ServiceCollectionDI
    {

        public static IServiceCollection AddConfigurationOptions(this IServiceCollection services)
        {
            services.AddOptions<MockAPIClientSettings>().BindConfiguration("TSMockAPI").ValidateDataAnnotations().ValidateOnStart();
            return services;
        }

        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
     

        public static IServiceCollection AddValidatorDependencies(this IServiceCollection services)
        {
            services.AddScoped<IValidator<ProductFilterRequestDto>, ProductFilterRequestValidator>();
            services.AddScoped<IValidator<CreateProductRequestDto>, CreateProductRequestValidator>();
            services.AddScoped<IValidator<DeleteProductRequestDto>, DeleteProductRequestValidator>();

            return services;
        }

        public static IServiceCollection AddHealthCheck(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddExternalHttpClient(this IServiceCollection services)
        {


            services.AddHttpClient<IExternalAPIService, MockExternalAPIService>((serviceProvider, client) =>
            {
                MockAPIClientSettings settings = serviceProvider
                .GetRequiredService<IOptions<MockAPIClientSettings>>().Value;

                client.DefaultRequestHeaders.Add("User-Agent", settings.UserAgent);
                client.DefaultRequestHeaders.Add("Accept", settings.AcceptHeader);
                client.BaseAddress = new Uri(settings.BaseUrl);
            });

            return services;
        }
    }
}
