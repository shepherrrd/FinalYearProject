using Microsoft.AspNetCore.Http;
using FinalYearProject.Infrastructure.Data.Models;
using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Infrastructure.Utilities.Enums;

namespace FinalYearProject.Infrastructure.Infrastructure.Services.Interfaces;

public interface IUtilityService
{
    string GenerateRandomNumber(int length);
    string GetPDFBase64String(string htmlBody);

    BaseResponse<string> VerifySdtmDatasetFormat(IFormFile file);
    BaseResponse<string> VerifyICDDatasetFormat(IFormFile file);
     string ConvertToCsv<T>(IEnumerable<T> records);
    string ConvertToBase64(string input);

}

