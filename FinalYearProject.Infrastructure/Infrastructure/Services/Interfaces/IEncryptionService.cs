

using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Data.Models;
using System.Security.Cryptography;

namespace FinalYearProject.Infrastructure.Infrastructure.Services.Interfaces;

public interface  IEncryptionService
{
    BaseResponse<EncryptionEntity> GenerateEncryptionKey();
    BaseResponse<byte[]> EncryptDataAsync(string data,string publickey, string expontKey);

    BaseResponse<string> DecryptdataAsync(byte[] data,string privatekey);
}
public interface IHybridEncryption
{
    BaseResponse<byte[]> EncryptData(string data, RSAParameters publicKey);
    BaseResponse<string> DecryptData(byte[] data, RSAParameters privateKey);
}

public interface IKeyGenerator
{
    (RSAParameters publicKey, RSAParameters privateKey) GenerateKeys();
}
