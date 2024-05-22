using Infrastructure.Search.Pagination;

namespace Infrastructure.Search;

public interface IProjectionGateway
{
    ValueTask<IPagedResult<TProjection>> SearchAsync<TProjection>(SearchRequest request, CancellationToken token)
        where TProjection : class;

    Task IndexAsync<TDocument>(TDocument document, CancellationToken token)
        where TDocument : notnull;
}