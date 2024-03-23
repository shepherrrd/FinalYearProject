using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalYearProject.Infrastructure.Data.Models
{
    internal class Dtos
    {
    }

    public class DataReuqestDto : BaseEntity
    {
        public string Description { get; set; } = default!;
        public bool IsApproved { get; set; }
        public int IrbProposalId { get; set; }
        public string Name { get; set; } = default!;
        public DataRequestSatusEnum status { get; set; }
    }
    public class MeetchopraValidResponse
    {
        public bool status { get; set; }
    }
    public class SendGridErrorResponse
    {
        public IEnumerable<SendGridSingleEmailResponseError>? Errors { get; set; }
    }

    public class SendGridSingleEmailResponseError
    {
        public string? Message { get; set; }
        public string? Field { get; set; }
        public string? Help { get; set; }
    }
    public class ErrorResponse
    {
        public bool Status { get; set; } = false;
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new();
        public string? TraceId { get; set; }
    }

    public class SingleEmailRequest
    {
        public string? RecipientName { get; set; }
        public string? RecipientEmailAddress { get; set; }
        public string? EmailSubject { get; set; }
        public string? HtmlEmailBody { get; set; }
        public string? PlainEmailBody { get; set; }
        public List<string>? AttachementBase64String { get; set; }
        public List<string>? AttachementName { get; set; }
        public List<string>? AttachementType { get; set; }
    }
    public class DataEmailRequest
    {
        public string? RecipientName { get; set; }
        public string? RecipientEmailAddress { get; set; }
        public string? EmailSubject { get; set; }
        public string? HtmlEmailBody { get; set; }
        public string? PlainEmailBody { get; set; }
        public string? SDTMAttachementBase64String { get; set; }
        public string? SDTMAttachementName { get; set; }
        public string? SDTMAttachementType { get; set; }
        public string? ICDAttachementBase64String { get; set; }
        public string? ICDAttachementName { get; set; }
        public string? ICDAttachementType { get; set; }
    }

    public class RequestResponse : BaseEntity
    {
        public string Description { get; set; } = default!;
        public string IrbProposal { get; set; } = default!;
        public string IrbApproval { get; set; } = default!;
        public string Reason { get; set; } = default!;
        public  string Passport { get; set; } = default!;
        public string Name { get; set; } = default!;
    }
    public class MultipleEmailRequest
    {
        public string? RecipientName { get; set; }
        public IEnumerable<string>? RecipientEmailAddresses { get; set; }
        public string? EmailSubject { get; set; }
        public string? HtmlEmailBody { get; set; }
        public string? PlainEmailBody { get; set; }
        public string? AttachementBase64String { get; set; }
        public string? AttachementName { get; set; }
        public string? AttachementType { get; set; }
    }

}
