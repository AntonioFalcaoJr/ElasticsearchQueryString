using System.Text.Json.Serialization;
using FluentValidation;
using Infrastructure.Search;
using Infrastructure.Search.DependencyInjection;
using WebAPI;
using static Microsoft.AspNetCore.Http.Results;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options 
        => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddSwaggerGen(options 
    => options.SchemaFilter<EnumSchemaFilter>());

builder.Services.AddProjections();
builder.Services.AddHealthChecks();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options => options.EnableTryItOutByDefault());

app.MapPost("/seed", (IProjectionGateway gateway, CancellationToken token)
    => new Seeder(gateway).SeedAsync(token));

app.MapGet("/search", async ([AsParameters] SearchingRequest search) =>
{
    var validation = await search.Validator.ValidateAsync(search, search.Token);
    if (!validation.IsValid) return ValidationProblem(validation.ToDictionary());

    var pagedResult = await search.Gateway
        .SearchAsync<object>(search with { Query = search.Query }, search.Token);
    
    return pagedResult.Items.Any() ? Ok(pagedResult) : NoContent();
});

app.MapHealthChecks("/healthz").ShortCircuit();

app.MapGet("/", () => Redirect("/swagger")).ShortCircuit().ExcludeFromDescription();

await app.RunAsync();