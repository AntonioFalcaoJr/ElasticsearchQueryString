using Bogus;
using Infrastructure.Search;

public class Seeder(IProjectionGateway gateway)
{
    public void SeedAsync()
    {
        Persons(10).ForEach(person => gateway.IndexAsync(person, new()));
        Companies(10).ForEach(company => gateway.IndexAsync(company, new()));
        Products(10).ForEach(product => gateway.IndexAsync(product, new()));
    }

    private List<Person> Persons(int count)
        => new Faker<Person>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .Generate(count);

    private List<Company> Companies(int count)
        => new Faker<Company>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Company.CompanyName())
            .RuleFor(c => c.Address, f => f.Address.StreetAddress())
            .Generate(count);

    private List<Product> Products(int count)
        => new Faker<Product>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Commerce.Price())
            .Generate(count);
}