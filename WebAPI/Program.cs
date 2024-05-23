using FluentValidation;
using Infrastructure.Search;
using Infrastructure.Search.DependencyInjection;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProjections();
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<SearchingRequestValidator>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options => options.EnableTryItOutByDefault());

app.MapPost("/seed", (IProjectionGateway gateway, CancellationToken token)
    => new Seeder(gateway).SeedAsync(token));

app.MapGet("/search", async ([AsParameters] SearchingRequest request) =>
{
    var result = await request.Validator.ValidateAsync(request, request.Token);
    if (!result.IsValid) return Results.BadRequest(result.Errors.Select(error => error.ErrorMessage));

    var pagedResult = await request.Gateway
        .SearchAsync<object>(request with { Query = request.Query }, request.Token);

    return pagedResult.Items.Any()
        ? Results.Ok(pagedResult)
        : Results.NoContent();
});

await app.RunAsync();