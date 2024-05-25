namespace WebAPI.Models;

public record Person
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = "Undefined";
    public string Email { get; set; } = "Undefined";
}