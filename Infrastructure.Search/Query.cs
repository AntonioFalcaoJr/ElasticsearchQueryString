namespace Infrastructure.Search;

public record Query
{
    private string Value { get; }

    public Query(string value, Guid tenantId)
        => Value = $"({value}) AND tenantId.keyword:{tenantId}";

    public static implicit operator string(Query query) => query.Value;
    public static implicit operator Query((string value, Guid tenantId) query) => new(query.value, query.tenantId);
}