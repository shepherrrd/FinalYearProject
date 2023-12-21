using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Data.Models;

namespace FinalYearProject.Infrastructure.Services.Interfaces
{
    public interface ISendGridService
    {
        Task<BaseResponse> SendEmailAsync(SingleEmailRequest request);
        Task<BaseResponse> SendEmailToMultipleRecipientsAsync(MultipleEmailRequest request);
    }
}
