
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using Microsoft.AspNetCore.Identity;

namespace FinalYearProject.Infrastructure.Data.Entities;

public class DataAggregatorUser : IdentityUser<long>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public UserType UserType { get; set; }
    public string? EmailAddress { get; set; }
}
