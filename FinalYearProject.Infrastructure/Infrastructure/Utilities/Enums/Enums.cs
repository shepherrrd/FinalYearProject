
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