namespace Infrastructure.Search.Pagination;

public interface IPagedResult<out TProjection>
{
    IEnumerable<IProjection<TProjection>> Items { get; }
    Page Page { get; }
}