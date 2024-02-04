
using System.ComponentModel;
using System.Reflection;

namespace FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;

public enum UserType
{
    [Description("Admin")]
    Admin =1,
    [Description("Hospital")]
    Hospital,
    [Description("Research Center")]
    ResearchCenter
}
public enum MedicalRecordTypeEnum
{
    [Description("Health")]
    Health =1
}
public enum OtpCodeStatusEnum
{
    [Description("Sent")]
    Sent = 1,

    [Description("Verified")]
    Verified,

    [Description("Expired")]
    Expired,

    [Description("Invalidated")]
    Invalidated
}

public enum OtpVerificationPurposeEnum
{
   

    [Description("Email Confirmation")]
    EmailConfirmation,

    [Description("Password Reset")]
    PasswordReset
}

public enum OtpRecipientTypeEnum
{
    [Description("Phone Number")]
    Phone = 1,

    [Description("Email Address")]
    Email
}

public enum OtpCodeLengthEnum
{
    [Description("Four")]
    Four = 1,

    [Description("Six")]
    Six
}
public enum AccountStatusEnum
{
    [Description("Active")]
    Active = 1,
    [Description("Suspended")]
    Suspended,
    [Description("InActive")]
    InActive
}
public enum EmailTitleEnum
{
    EMAILVERIFICATION = 1,
    PASSWORDRESET,
    BVNVERIFICATION,
    WELCOME,
    TRANSACTIONSTATEMENT,
    DOCUMENTUPLOAD,
    KYCCOMPLETED,
    INTERNATIONALWALLETREQUEST,
    CREATENEWUSER,
    KYCAPPROVED,
    KYCREJECTED,
    CONNECTEDCARDAPPROVED,
    CONNECTEDCARDREJECTED
}

public static class Enums
{
    public static string GetDescription(this Enum GenericEnum)
    {
        Type genericEnumType = GenericEnum.GetType();
        MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
        if ((memberInfo != null && memberInfo.Length > 0))
        {
            var _Attribs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if ((_Attribs != null && _Attribs.Length > 0))
            {
                return ((DescriptionAttribute)_Attribs.ElementAt(0)).Description;
            }
        }

        return GenericEnum.ToString();
    }
}