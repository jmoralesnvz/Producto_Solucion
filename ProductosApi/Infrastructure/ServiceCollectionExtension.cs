
using ProductosApi.Services;

namespace ProductosApi.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
        }

        public static void ConfigureCors(this IServiceCollection services, string configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(configuration)
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }
    }
}
