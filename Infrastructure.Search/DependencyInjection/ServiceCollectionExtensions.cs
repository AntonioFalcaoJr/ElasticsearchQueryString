using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Uri = System.Uri;

namespace WebAPP.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProjections(this IServiceCollection services)
    {
        services.AddScoped<IProjectionGateway, ProjectionGateway>();

        services.AddSingleton(provider =>
        {
            var connectionString = provider.GetRequiredService<IConfiguration>().GetConnectionString("Elasticsearch");
            
            var settings = new ElasticsearchClientSettings(new Uri(connectionString))
                .Authentication(new BasicAuthentication("search", "search"));
            
            return new ElasticsearchClient(settings);
        });

        return services;
    }
}