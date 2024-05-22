using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Elastic.Transport.Extensions;
using WebAPP.Infrastructure.Pagination;

namespace WebAPP.Infrastructure;

public record Paging(int Number, int Size);

public record Query(string Value)
{
    public static implicit operator string(Query query) => query.Value;
}

public record SearchRequest(Query Query, Indices Indices, Fields Fields, Paging Paging);
public record SearchResponse<T>(IEnumerable<T> Items);

public interface IProjectionGateway
{
    ValueTask<IPagedResult<TProjection>> SearchAsync<TProjection>(SearchRequest request, CancellationToken token)
        where TProjection : notnull;

    Task IndexAsync<TProjection>(TProjection projection, CancellationToken token)
        where TProjection : notnull;
}

public class ProjectionGateway(ElasticsearchClient client) : IProjectionGateway
{
    public async ValueTask<IPagedResult<TProjection>> SearchAsync<TProjection>
        (SearchRequest request, CancellationToken token) where TProjection : notnull
    {
        try
        {
            var query =  client.SearchAsync<TProjection>(search => search
                    .Index(request.Indices)
                    .Query(descriptor => descriptor
                        .QueryString(queryString => queryString
                            .Fields(request.Fields)
                            .Query(request.Query)))
                    .From(request.Paging.Number)
                    .Size(request.Paging.Size + 1), token);

            var x = client.RequestResponseSerializer.SerializeToString(query, SerializationFormatting.Indented);

            var response = await query;
            
            if (response.IsValidResponse is false)
            {
                throw new InvalidOperationException("Invalid response from Elasticsearch");
            }

            return PagedResult<TProjection>.Create(response.Documents, request.Paging);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task IndexAsync<TProjection>(TProjection projection, CancellationToken token) where TProjection : notnull
        => client.IndexAsync(projection, projection.GetType().Name.ToLower(), token);
}