
namespace FinalYearProject.Infrastructure.Data.Entities;

public class BaseEntity
{
    public long Id { get; set; }
    public DateTimeOffset TimeCreated { get; set; }
    public DateTimeOffset TimeUpdated { get; set; }
}
public record BaseRecordEntity
{
    public long Id { get; set; } = default!;
    public DateTimeOffset TimeCreated { get; set; } = default!;
    public DateTimeOffset TimeUpdated { get; set; } = default!;
}

public class Country : BaseEntity
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? CCode { get; set; }
    public string? IsoCode { get; set; }
    public string? Currency { get; set; }
    public string? Icon { get; set; }
}

public class JwtSettings
{
#nullable disable
    public string Secret { get; set; }
    public int ExpiryMinutes { get; set; }
    public string Issuer { get; set; }
}