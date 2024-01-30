
using FinalYearProject.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalYearProject.Infrastructure.Infrastructure.Persistence;

public class FinalYearDBContext : IdentityDbContext<DataAggregatorUser, ApplicationRole, long>
{
    public FinalYearDBContext(DbContextOptions<FinalYearDBContext> options) : base(options)
    { }

    public DbSet<DataAggregatorUser> SystemUsers {  get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Request> RegistrationRequests { get; set; }

    public DbSet<ResearchCenterInfo> ResearchCenterInfo { get; set; }

    public DbSet<HospitalInfo> HospitalInfos { get; set; }
    public DbSet<Requests> HospitalRequests { get; set; }
    public DbSet<OtpVerification> OtpVerifications {  get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Seed();
    }
}

