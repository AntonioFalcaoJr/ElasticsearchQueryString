namespace WebAPI.Models;

public record Person
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}