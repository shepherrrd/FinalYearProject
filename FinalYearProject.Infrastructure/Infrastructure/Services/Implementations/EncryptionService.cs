
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
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = Convert.FromBase64String(privatekey)
            });
            decryptedData = rsa.Decrypt(data, false);
        }
        return new BaseResponse<string>(true,"Decrypted Successfully", Encoding.UTF8.GetString(decryptedData));
    }

    public BaseResponse<byte[]> EncryptDataAsync(string data,string publicKey)
    {
        byte[] encryptedData;
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = Convert.FromBase64String(publicKey)
            });
            byte[] dataToEncryptBytes = Encoding.UTF8.GetBytes(data);
            encryptedData = rsa.Encrypt(dataToEncryptBytes, false);
        }
        return new BaseResponse<byte[]>(true,"Encryptd SUccessfully",encryptedData);
    }

    public BaseResponse<EncryptionEntity> GenerateEncryptionKey()
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048)) // 2048-bit key size
        {
            RSAParameters privateKey = rsa.ExportParameters(true);
            RSAParameters publicKey = rsa.ExportParameters(false);

            var response = new BaseResponse<EncryptionEntity>();
            if (privateKey.Modulus == null || publicKey.Modulus == null)
            {
                return new BaseResponse<EncryptionEntity>
                {
                    Status = false,
                    Message = "Error generating key pair: Modulus is null"
                };
            }
            response.Data!.PublicKey = Convert.ToBase64String(publicKey.Modulus);
            response.Data.PrivateKey = Convert.ToBase64String(privateKey.Modulus);
            response.Status = true;
            response.Message = "Keys generated";
            return response;
        }
    }
}
