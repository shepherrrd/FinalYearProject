using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalYearProject.Infrastructure.Infrastructure.Auth
{
    public class AuthorizationPolicyCodes
    {
        public const string BackofficeUserPolicyCode = "BackofficePolicyCode";
        public const string UserPolicyCode = "UserPolicyCode";
    }

    public class BackOfficePrivileges
    {
        public const string Users = "Users";
        public const string ActivityLogs = "ActivityLogs";
        public const string Dashboards = "Dashboard";

        public string User { get; private set; } = Users;
        public string ActivityLog { get; private set; } = ActivityLogs;
        public string Dashboard { get; private set; } = Dashboards;
    }
}
