using Infrastructure.Search.Pagination;

namespace Infrastructure.Search;

public interface IProjectionGateway
{
    ValueTask<IPagedResult<IProjection<THit>>> SearchAsync<THit>(SearchRequest request, CancellationToken token)
        where THit : class;

    Task IndexAsync<TDocument>(TDocument document, CancellationToken token)
        where TDocument : notnull;
}