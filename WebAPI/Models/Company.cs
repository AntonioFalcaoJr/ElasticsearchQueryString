namespace WebAPI.Models;

public record Company
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}