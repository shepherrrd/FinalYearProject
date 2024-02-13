
using EllipticCurve;
using FinalYearProject.Infrastructure.Data.Entities;
using FinalYearProject.Infrastructure.Data.Models;
using FinalYearProject.Infrastructure.Infrastructure.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace FinalYearProject.Infrastructure.Infrastructure.Services.Implementations;

public class EncryptionService : IEncryptionService
{
    public BaseResponse<string> DecryptdataAsync(byte[] data, string privatekey)
    {
        byte[] decryptedData;
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(privatekey);
            decryptedData = rsa.Decrypt(data, false);
        }
        return new BaseResponse<string>(true,"Decrypted Successfully", Encoding.UTF8.GetString(decryptedData));
    }

    public BaseResponse<byte[]> EncryptDataAsync(string data,string publicKey)
    {
        byte[] encryptedData;
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publicKey);
            byte[] dataToEncryptBytes = Encoding.UTF8.GetBytes(data);
            encryptedData = rsa.Encrypt(dataToEncryptBytes, false);
        }
        return new BaseResponse<byte[]>(true,"Encryptd SUccessfully",encryptedData);
    }

    public BaseResponse<EncryptionEntity> GenerateEncryptionKey()
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048)) // 2048-bit key size
        {
            var response = new BaseResponse<EncryptionEntity>();
            response.Data!.PublicKey = rsa.ToXmlString(false); 
            response.Data.PrivateKey = rsa.ToXmlString(true);
            response.Status = true;
            response.Message = "Keys generated";
            return response;
        }
    }
}
