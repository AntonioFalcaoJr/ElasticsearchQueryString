using System.Runtime.Serialization;
using Elastic.Clients.Elasticsearch;
using FluentValidation;
using Infrastructure.Search;
using Microsoft.OpenApi.Attributes;
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
        
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(ushort.MinValue)
            .WithMessage("PageNumber cannot be negative.");
        
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(ushort.MinValue)
            .WithMessage("PageSize cannot be negative.");

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
    Field[] Fields,
    Index[] Indexes,
    IValidator<SearchingRequest> Validator,
    CancellationToken Token)
{
    public static implicit operator SearchRequest(SearchingRequest request)
        => new(
            Query: (request.Query, request.TenantId),
            Indices: request.Indexes.Select(index => index.ToString().ToLower()).ToArray(),
            Fields: request.Fields.Select(field => field.ToString().ToLower()).ToArray()!,
            Paging: new(request.PageNumber ?? 0, request.PageSize ?? 0));
}

public enum Index { Person, Company, Product }

public enum Field { Name, Address, Email, Price }