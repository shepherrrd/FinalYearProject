

using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Auth;
using FinalYearProject.Infrastructure.Infrastructure.Auth.JWT;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.Text;

namespace FinalYearProject.Infrastructure.Infrastructure.Persistence;

public  static class Extensions
{
    
    public static IServiceCollection RegisterPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FinalYearDBContext>(option =>
        {
            option.UseNpgsql(configuration.GetConnectionString("FinalYearDB"));

        });


        return services;
    }
    public static void Seed(this ModelBuilder modelBuilder)
    {
        
        

        #region PayUltra Users
        modelBuilder.Entity<DataAggregatorUser>().HasData(
            new DataAggregatorUser
            {
                AccountStatus = AccountStatusEnum.Active,
                ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                Email = "technology@payultra.com",
                EmailConfirmed = true,
                FirstName = "PayUltra",
                Id = 1,
                LastName = "Administrator",
                NormalizedEmail = "TECHNOLOGY@DATAAGGREGATOR.COM",
                NormalizedUserName = "Shepherd",
                PasswordHash = new PasswordHasher<DataAggregatorUser>().HashPassword(null!, "password"),
                SecurityStamp = GenerateSecurityStamp(),
                TimeCreated = DateTimeOffset.UtcNow,
                TimeUpdated = DateTimeOffset.UtcNow,
                UserName = "technology@payultra.com",
                UserType = UserType.Admin
            }
        );
        #endregion

        #region User Claims
        modelBuilder.Entity<IdentityUserClaim<long>>().HasData(
            new IdentityUserClaim<long>
            {
                ClaimType = BackOfficePrivileges.ActivityLogs,
                ClaimValue = BackOfficePrivileges.ActivityLogs,
                Id = 1,
                UserId = 1
            },
            
            new IdentityUserClaim<long>
            {
                ClaimType = BackOfficePrivileges.Dashboards,
                ClaimValue = BackOfficePrivileges.Dashboards,
                Id = 4,
                UserId = 1
            },
           
            new IdentityUserClaim<long>
            {
                ClaimType = BackOfficePrivileges.Users,
                ClaimValue = BackOfficePrivileges.Users,
                Id = 7,
                UserId = 1
            }
        );
        #endregion

        #region Country
        modelBuilder.Entity<Country>().HasData(
            new Country
            {
                Id = 1,
                Name = "Nigeria",
                IsoCode = "NG",
                CCode = "NGN",
                Currency = "₦",
                Code = "+234",
                Icon = "countryimages/nigeria.png",
                TimeCreated = DateTimeOffset.UtcNow,
                TimeUpdated = DateTimeOffset.UtcNow
            }
        );
        #endregion
    }

    private static readonly Func<string> GenerateSecurityStamp = () =>
    {
        var guid = Guid.NewGuid();
        return string.Concat(Array.ConvertAll(guid.ToByteArray(), b => b.ToString("X2")));
    };
}
