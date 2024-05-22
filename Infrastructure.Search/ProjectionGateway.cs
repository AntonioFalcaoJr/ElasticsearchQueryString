using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Infrastructure.Search.Pagination;

namespace Infrastructure.Search;

public record SearchRequest(Query Query, Indices Indices, Fields Fields, Paging Paging);

public class ProjectionGateway(ElasticsearchClient client) : IProjectionGateway
{
    public async ValueTask<IPagedResult<THit>> SearchAsync<THit>(SearchRequest request, CancellationToken token)
        where THit : class
    {
        var from = (request.Paging.Number - 1) * request.Paging.Size;
        var size = request.Paging.Size + 1;

        var response = await client
            .SearchAsync<THit>(search => search
                    .Index(request.Indices)
                    .Query(descriptor => descriptor
                        .QueryString(queryString => queryString
                            .Fields(request.Fields)
                            .Query(request.Query)))
                    .Highlight(highlight => highlight
                        .Fields(dictionary => dictionary
                            .Add("*", new HighlightFieldDescriptor<THit>())))
                    .From(from)
                    .Size(size),
                token);

        if (response.IsValidResponse is false)
            throw new InvalidOperationException(response.ElasticsearchServerError?.Error.Reason);

        var projections = response.Hits.Select(hit => new Projection<THit>(hit.Source!, hit.Highlight!));
        
        return PagedResult<THit>.Create(projections, request.Paging);
    }

    public Task IndexAsync<TDocument>(TDocument document, CancellationToken token) where TDocument : notnull
        => client.IndexAsync(document, document.GetType().Name.ToLower(), token);
}