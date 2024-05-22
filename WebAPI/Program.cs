using Infrastructure.Search;
using Infrastructure.Search.DependencyInjection;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProjections();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options => options.EnableTryItOutByDefault());

app.MapPost("/seed", (IProjectionGateway gateway, CancellationToken token)
    => new Seeder(gateway).SeedAsync(token));

app.MapGet("/search", ([AsParameters] SearchingRequest request)
    => request.Gateway.SearchAsync<object>(request with { Query = request.Query }, request.Token));

await app.RunAsync();