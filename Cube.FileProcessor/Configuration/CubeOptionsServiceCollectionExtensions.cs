using Cube.FileProcessor.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cube.FileProcessor.Configuration
{
   public static class CubeOptionsServiceCollectionExtensions
    {
        public const string StorageSectionName = "Storage";

        public static IServiceCollection AddCpaOptions(this IServiceCollection services)
        {
            services
                .AddOptions<FileShareOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection(StorageSectionName).Bind(settings);
                });
        
                services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<FileShareOptions>>().Value);
            return services;
        }
    }
}
