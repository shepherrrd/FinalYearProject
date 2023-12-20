using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Auth.JWT;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FinalYearProject.Infrastructure.Infrastructure.Auth
{
    public static  class Extensions
    {
        public static IServiceCollection RegisterIdentity(this IServiceCollection services)
        {
            services.AddIdentity<DataAggregatorUser, ApplicationRole>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 6;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 1;

                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<FinalYearDBContext>()
            .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromMinutes(15));

            return services;
        }

        public static IServiceCollection RegisterAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtSettings:Secret"]!)),
                };
            });

            var jwtSettings = new JwtSettings
            {
                Secret = configuration["JwtSettings:Secret"],
                ExpiryMinutes = int.Parse(configuration["JwtSettings:ExpiryMinutes"] ?? "0"),
                Issuer = configuration["JwtSettings:Issuer"]
            };
            services.AddSingleton(jwtSettings);

            return services;
        }

        public static IServiceCollection RegisterAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("BackofficePolicyCode", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim("PolicyCode", AuthorizationPolicyCodes.BackofficeUserPolicyCode);
                });

                options.AddPolicy("UserPolicyCode", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim("PolicyCode", AuthorizationPolicyCodes.UserPolicyCode);
                });
            });
            //services.AddScoped<ValidTokenFilter>();

            return services;
        }

        public static IServiceCollection RegisterJwt(this IServiceCollection services, IConfiguration configuration)
        {
            //var authenticationProviderKey = "JwtBearer";
            var jwtSettings = new JwtSettings();

            configuration.Bind(nameof(JwtSettings), jwtSettings);

            services.AddSingleton(jwtSettings);
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddSingleton<IJwtHandler, JwtHandler>();

            return services;
        }
    }
}
