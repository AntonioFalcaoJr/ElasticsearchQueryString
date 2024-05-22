using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Infrastructure.Search.Pagination;

namespace Infrastructure.Search;

public record SearchRequest(Query Query, Indices Indices, Fields Fields, Paging Paging);

public class ProjectionGateway(ElasticsearchClient client) : IProjectionGateway
{
    public async ValueTask<IPagedResult<TProjection>> SearchAsync<TProjection>(SearchRequest request, CancellationToken token)
        where TProjection : class
    {
        var from = (request.Paging.Number - 1) * request.Paging.Size;
        var size = request.Paging.Size + 1;

        var response = await client
            .SearchAsync<TProjection>(search => search
                    .Index(request.Indices)
                    .Query(descriptor => descriptor
                        .QueryString(queryString => queryString
                            .Fields(request.Fields)
                            .Query(request.Query)))
                    .Highlight(highlight => highlight
                        .Fields(dictionary => dictionary
                            .Add("*", new HighlightFieldDescriptor<TProjection>())))
                    .From(from)
                    .Size(size),
                token);

        if (response.IsValidResponse is false)
            throw new InvalidOperationException(response.ElasticsearchServerError?.Error.Reason);

        var projections = response.Hits.Select(hit => new Projection<TProjection>(hit.Source!, hit.Highlight!));
        
        return PagedResult<TProjection>.Create(projections, request.Paging);
    }

    public Task IndexAsync<TDocument>(TDocument document, CancellationToken token) where TDocument : notnull
        => client.IndexAsync(document, document.GetType().Name.ToLower(), token);
}