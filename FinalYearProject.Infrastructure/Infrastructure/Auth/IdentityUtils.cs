using System.Security.Claims;
using System.Security.Principal;

namespace FinalYearProject.Infrastructure.Infrastructure.Auth
{
    public static class IdentityUtils
    {
        public static long GetProfileId(this IIdentity identity)
        {
            long.TryParse(GetClaimValue(identity, ClaimTypes.NameIdentifier), out long profileId);
            return profileId;
        }

        public static string GetEmail(this IIdentity identity)
        {
            return GetClaimValue(identity, "Email");
        }

        public static string GetPrivileges(this IIdentity identity)
        {
            return GetClaimValue(identity, "Privilege");
        }

        public static int GetUserType(this IIdentity identity)
        {
            _ = int.TryParse(GetClaimValue(identity, "UserType"), out int userType);
            return userType;
        }
        
        public static int GetAccountStatus(this IIdentity identity)
        {
            _ = int.TryParse(GetClaimValue(identity, "AccountStatus"), out int accountStatus);
            return accountStatus;
        }
        private static string GetClaimValue(this IEnumerable<Claim> claims, string claimType)
        {
            var claimsList = new List<Claim>(claims);
            var claim = claimsList.Find(c => c.Type == claimType);
            return claim?.Value ?? "";
        }

        private static string GetClaimValue(IIdentity identity, string claimType)
        {
            var claimIdentity = (ClaimsIdentity)identity;
            return claimIdentity.Claims.GetClaimValue(claimType);
        }
    }
}
