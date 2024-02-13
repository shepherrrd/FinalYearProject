

using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Data.Models;

namespace FinalYearProject.Infrastructure.Infrastructure.Services.Interfaces;

public interface  IEncryptionService
{
    BaseResponse<EncryptionEntity> GenerateEncryptionKey();
    BaseResponse<byte[]> EncryptDataAsync(string data,string publickey);

    BaseResponse<string> DecryptdataAsync(byte[] data,string privatekey);
}
