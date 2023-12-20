using FinalYearProject.Infrastructure.Data.Entities;

namespace FinalYearProject.Infrastructure.Services.Interfaces;

public interface ICloudinaryService
{
    Task<BaseResponse> UploadImageAsync(MemoryStream stream, string fileName);
    Task<BaseResponse> DownloadImageAsync(string imageUrl);
}
