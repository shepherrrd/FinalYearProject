
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;

namespace FinalYearProject.Infrastructure.Data.Entities;



    public class OtpVerification : BaseEntity
    {
        public string Recipient { get; set; } = default!;
        public OtpRecipientTypeEnum RecipientType { get; set; }
        public long UserId { get; set; }
        public string Code { get; set; } = default!;
        public OtpCodeStatusEnum Status { get; set; }
        public OtpVerificationPurposeEnum Purpose { get; set; }
        public DateTimeOffset? ConfirmedOn { get; set; }
    }

