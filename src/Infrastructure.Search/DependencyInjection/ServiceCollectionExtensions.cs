using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uri = System.Uri;

namespace Infrastructure.Search.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProjections(this IServiceCollection services)
    {
        services.AddScoped<IProjectionGateway, ProjectionGateway>();

        services.AddSingleton(provider =>
        {
            var connectionString = provider.GetRequiredService<IConfiguration>().GetConnectionString("Elasticsearch");
            
            var settings = new ElasticsearchClientSettings(new Uri(connectionString!))
                .Authentication(new BasicAuthentication("search", "search"))
                .EnableHttpCompression(false);
            
            return new ElasticsearchClient(settings);
        });

        return services;
    }
}