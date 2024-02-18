using FinalYearProject.Infrastructure.Data.Entities;

namespace FinalYearProject.Infrastructure.Infrastructure.Services.Interfaces;

public interface IAccountService
{
    Task<BaseResponse<OtpRequestResult>> SendOTPAsync(SendOTPRequest request, CancellationToken cancellationToken);
    Task<BaseResponse> ValidateOTPCodeAsync(ValidateOtpRequest otpRequest, CancellationToken cancellationToken);
    Task<BaseResponse> SendHospitalWelcomeEmailAsync(SendHospitalKeysRequest request, CancellationToken cancellationToken);
    Task<BaseResponse> SendMedicalRecordsAsync(SendMedicalDataRequest request, CancellationToken cancellationToken);
}
