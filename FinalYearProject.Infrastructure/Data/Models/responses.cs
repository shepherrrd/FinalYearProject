using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalYearProject.Infrastructure.Data.Models;

public class ValidationResultModel
{
    public bool Status { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}
public class LoginResponse
{
#nullable disable
    public long UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public UserType UserType { get; set; }
    public AccountStatusEnum AccountStatus { get; set; }
    public long RoleId { get; set; }
    public string RoleText { get; set; }
    public string Token { get; set; }
    public string Privileges { get; set; }
    public string RefferalMsgBody { get; set; }
    public long Expires { get; set; }
    public bool IsEmailVerified { get; set; }
}
public class JwtRequest
{
#nullable disable
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string EmailAddress { get; set; }
    public UserType UserType { get; set; }
    public AccountStatusEnum AccountStatus { get; set; }
    public string ProfileCode { get; set; }
    public long RoleId { get; set; }
    public string RoleText { get; set; }
    public long UserId { get; set; }
    public bool IsEmailVerified { get; set; }
    public string Privileges { get; set; }
}
