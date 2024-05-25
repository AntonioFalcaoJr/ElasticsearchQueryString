namespace WebAPI.Models;

public record Product
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = "Undefined";
    public string Price { get; set; } = "Undefined";
}