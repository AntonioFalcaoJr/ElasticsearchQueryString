using Elastic.Clients.Elasticsearch;
using Infrastructure.Search;
using SearchRequest = Infrastructure.Search.SearchRequest;

namespace WebAPI;

public record SearchingRequest(IProjectionGateway Gateway, string Query, int PageSize, int PageNumber, CancellationToken Token)
{
    public static implicit operator SearchRequest(SearchingRequest request)
        => new(
            Query: request.Query,
            Indices: new IndexName[] { "person", "company", "product" },
            Fields: new Field[] { "name", "address", "email", "price" },
            Paging: new(request.PageNumber, request.PageSize));
}