namespace WebAPI;

public record Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Price { get; set; }
}