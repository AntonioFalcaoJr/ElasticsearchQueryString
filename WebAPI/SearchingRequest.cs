using Elastic.Clients.Elasticsearch;
using Infrastructure.Search;
using SearchRequest = Infrastructure.Search.SearchRequest;

public record SearchingRequest(IProjectionGateway Gateway, string Query, int PageSize, int PageNumber, CancellationToken Token)
{
    public static implicit operator SearchRequest(SearchingRequest request)
        => new((Query)request.Query,
            new IndexName[] { "person", "company", "product" },
            new Field[] { "name", "address", "email", "price" },
            new(request.PageNumber, request.PageSize));
}