
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using Microsoft.AspNetCore.Identity;

namespace FinalYearProject.Infrastructure.Data.Entities;

public class DataAggregatorUser : IdentityUser<long>
{
    public string? FirstName { get; set; }
    public AccountStatusEnum? AccountStatus { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public UserType UserType { get; set; }
    public int? CountryID { get; set; }
    public long? HospitalInfo { get; set; }
    public long? ResearchCenterInfo { get; set; }
    public string? LastLoginDevice { get; set; }
    public DateTimeOffset LastLogin { get; set; }
    public DateTimeOffset TimeCreated { get; set; }
    public DateTimeOffset TimeUpdated { get; set; }

}

public class ApplicationRole : IdentityRole<long>
{
    public string? Description { get; set; }
    public DateTimeOffset TimeCreated { get; set; }
    public DateTimeOffset TimeUpdated { get; set; }
}

public class HospitalInfo
{
    public long ID { get; set; }
    public long UserId { get; set; }
    public string? HospitalName { get; set; }
    public string? HospitalWebsite { get; set; }

    public int? CacDocumentID { get; set; }
    public int? NafdacDocumentID { get; set; }

}

public class ResearchCenterInfo
{
    public long ID { get; set; }
    public long UserId { get; set; }
    public string? Title { get; set; }
    public string? Institution { get; set; }

    public int? PassportID { get; set; }
    public int? DegreeID { get; set; }
    public int? ResearchProposalID { get; set; }
    public int? IRBApprovalID { get; set; }
}
