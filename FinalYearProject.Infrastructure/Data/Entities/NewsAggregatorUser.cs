
using Microsoft.AspNetCore.Identity;

namespace FinalYearProject.Infrastructure.Data.Entities;

public class NewsAggregatorUser : IdentityUser<long>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? HandleName { get; set; }
}
