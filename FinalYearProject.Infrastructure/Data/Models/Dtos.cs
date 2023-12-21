﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalYearProject.Infrastructure.Data.Models
{
    internal class Dtos
    {
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
    public class SingleEmailRequest
    {
        public string? RecipientName { get; set; }
        public string? RecipientEmailAddress { get; set; }
        public string? EmailSubject { get; set; }
        public string? HtmlEmailBody { get; set; }
        public string? PlainEmailBody { get; set; }
        public string? AttachementBase64String { get; set; }
        public string? AttachementName { get; set; }
        public string? AttachementType { get; set; }
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
