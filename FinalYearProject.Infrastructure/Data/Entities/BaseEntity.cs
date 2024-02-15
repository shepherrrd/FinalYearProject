
using FinalYearProject.Infrastructure.Data.Models;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;

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

public class BaseResponse
{
    public bool Status { get; set; }
    public string? Message { get; set; }

    public BaseResponse(bool status , string message)
    {
        Status = status;
        Message = message;
    }
}

public class  LoginBaseResponse 
{
    public bool Status { get; set; }
    public string? Message { get; set; }
    public LoginResponse? Data { get; set; }
    public LoginBaseResponse(bool status, string message)
    {
        Status = status;
        Message = message;
    }
    public LoginBaseResponse(bool status, string message, LoginResponse data)
    {
        Status = status;
        Message = message;
        Data = data;
    }
}

public class EmailBodyRequest
{
    public string? FirstName { get; set; }
    public string? Email { get; set; }
    public EmailTitleEnum EmailTitle { get; set; }
}

public class EmailBodyResponse
{
    public string? PlainBody { get; set; }
    public string? HtmlBody { get; set; }
}
public class SendOTPRequest
{
    public long UserId { get; set; }
    public string FirstName { get; set; } = default!;
    public string Recipient { get; set; } = default!;
    public OtpCodeLengthEnum OtpCodeLength { get; set; }
    public OtpRecipientTypeEnum RecipientType { get; set; }
    public OtpVerificationPurposeEnum Purpose { get; set; }
}
public class SendHospitalKeysRequest
{
    public long UserId { get; set; } 
    public string? FirstName { get; set; }
    public string? Email { get; set; }
    public string? PrivateKey { get; set; }
    public string? PublicKey { get; set; }
}

public class OtpRequestResult
{
    public long UserId { get; set; }
    public string? Recipient { get; set; }      //phone no or email
}
public class ValidateOtpRequest
{
    public long UserId { get; set; }
    public string? Code { get; set; }
    public OtpVerificationPurposeEnum Purpose { get; set; }
}

public class BaseResponse<T>
{
    public bool Status { get; set; }
    public string? Message { get; set; }

    
    public T? Data { get; set; }
    public  BaseResponse()
    {

    }
    public BaseResponse(bool status, string message)
    {
        Status = status;
        Message = message;
    }
    public BaseResponse(bool status, string message, T data)
    {
        Status = status;
        Message = message;
        Data = data;
    }
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
