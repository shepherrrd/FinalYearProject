using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Data.Models;

namespace FinalYearProject.Infrastructure.Services.Interfaces
{
    public interface IEmailService
    {
        Task<BaseResponse> CheckDisposableEmailAsync(string email);
        Task<BaseResponse> SendEmailAsync(SingleEmailRequest request);
        Task<BaseResponse> SendMultipleEmailAsync(MultipleEmailRequest request);
        Task<BaseResponse<EmailBodyResponse>> GetEmailBody(EmailBodyRequest request);
        Task<string> GetTemplateFileAsync(string folderName, string fileName);
    }
}
