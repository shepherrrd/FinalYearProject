using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FinalYearProject.Infrastructure.Services.Implementations;

public class CloudinaryService : ICloudinaryService
{
    private readonly IConfiguration _config;
    private readonly ILogger<CloudinaryService> _logger;
    public CloudinaryService(IConfiguration config,
        ILogger<CloudinaryService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<BaseResponse> DownloadImageAsync(string imageUrl)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(imageUrl);
            response.EnsureSuccessStatusCode();
            var imageBytes = await response.Content.ReadAsByteArrayAsync();
            var base64String = Convert.ToBase64String(imageBytes);

            return new BaseResponse(true, base64String);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "CLOUDINARY_SERVICE => Failed To Download Image Image");

            return new BaseResponse(false, "Unable To Get Image ");
        }
    }

    public async Task<BaseResponse> UploadImageAsync(MemoryStream stream, string fileName)
    {
        try
        {
            string cloudName = $"{_config["Cloudinary:CloudName"]}";
            string ApiKey = $"{_config["Cloudinary:ApiKey"]}";
            string SecretKey = $"{_config["Cloudinary:SecretKey"]}";
            var account = new Account(cloudName, ApiKey, SecretKey);
            Cloudinary cloudinary = new(account);
            stream.Seek(0, SeekOrigin.Begin);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, stream),
                PublicId = fileName,
                Overwrite = true,
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new BaseResponse(false, "");
            }
            _logger.LogInformation("CLOUDINARY_SERVICE => uploading image");
            stream.Dispose();

            return new BaseResponse(true, uploadResult.SecureUrl.ToString());

        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "CLOUDINARY_SERVICE => Failed To upload Image");
            stream.Dispose();

            return new BaseResponse(false, "Unable to Upload Image");
        }
    }

    public async Task<BaseResponse> UploadFilesAsync(MemoryStream stream, string fileName)
    {
        try
        {
            string cloudName = $"{_config["Cloudinary:CloudName"]}";
            string ApiKey = $"{_config["Cloudinary:ApiKey"]}";
            string SecretKey = $"{_config["Cloudinary:SecretKey"]}";
            var account = new Account(cloudName, ApiKey, SecretKey);
            Cloudinary cloudinary = new(account);
            stream.Seek(0, SeekOrigin.Begin);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, stream),
                PublicId = fileName,
                Overwrite = true,
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new BaseResponse(false, "");
            }
            _logger.LogInformation("CLOUDINARY_SERVICE => uploading image");
            stream.Dispose();

            return new BaseResponse(true, uploadResult.SecureUrl.ToString());

        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "CLOUDINARY_SERVICE => Failed To upload Image");
            stream.Dispose();

            return new BaseResponse(false, "Unable to Upload Image");
        }
    }
}

