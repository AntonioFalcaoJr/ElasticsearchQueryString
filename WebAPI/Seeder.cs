using Bogus;
using Infrastructure.Search;
using WebAPI.Models;
using Person = WebAPI.Models.Person;

namespace WebAPI;

public class Seeder(IProjectionGateway gateway)
{
    public Task SeedAsync(CancellationToken token)
        => Task.WhenAll(
            Persons(10).Select(person => gateway.IndexAsync(person, token)).Concat(
            Companies(10).Select(company => gateway.IndexAsync(company, token))).Concat(
            Products(10).Select(product => gateway.IndexAsync(product, token))));

    private static List<Person> Persons(int count)
        => new Faker<Person>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .Generate(count);

    private static List<Company> Companies(int count)
        => new Faker<Company>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Company.CompanyName())
            .RuleFor(c => c.Address, f => f.Address.StreetAddress())
            .Generate(count);

    private static List<Product> Products(int count)
        => new Faker<Product>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Commerce.Price())
            .Generate(count);
}