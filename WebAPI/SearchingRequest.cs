using Elastic.Clients.Elasticsearch;
using FluentValidation;
using Infrastructure.Search;
using SearchRequest = Infrastructure.Search.SearchRequest;

namespace WebAPI;

public class SearchingRequestValidator : AbstractValidator<SearchingRequest>
{
    public SearchingRequestValidator()
    {
        RuleFor(x => x.Query)
            .NotEmpty()
            .WithMessage("Query is required.");

        RuleFor(x => x.TenantId)
            .NotEmpty()
            .WithMessage("TenantId is required.");

        RuleFor(query => query)
            .Must(request => !request.Query.Contains('^'))
            .WithMessage("Query boosting (^), is not allowed.");

        RuleFor(query => query)
            .Must(request => !request.Query.Contains(':'))
            .WithMessage("Field-specific search (field:value), is not allowed.");

        RuleFor(query => query)
            .Must(request => request.Query.Trim() is not "*")
            .WithMessage("Query cannot be only a wildcard (*).");
    }
}

public record SearchingRequest(
    IProjectionGateway Gateway,
    string Query,
    Guid TenantId,
    ushort? PageNumber,
    ushort? PageSize,
    IValidator<SearchingRequest> Validator,
    CancellationToken Token)
{
    public static implicit operator SearchRequest(SearchingRequest request)
        => new(
            Query: (request.Query, request.TenantId),
            Indices: new IndexName[] { "person", "company", "product" },
            Fields: new Field[] { "name", "address", "email", "price" },
            Paging: new(request.PageNumber ?? 0, request.PageSize ?? 0));
}