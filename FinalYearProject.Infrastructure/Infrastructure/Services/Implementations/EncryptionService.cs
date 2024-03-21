
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

    public BaseResponse<byte[]> EncryptDataAsync(string data,string publicKey, string exponentkey)
    {
        byte[] encryptedData;
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = Convert.FromBase64String(publicKey),
                Exponent = Convert.FromBase64String(exponentkey) // You need to include the Exponent here
            });
            byte[] dataToEncryptBytes = Encoding.UTF8.GetBytes(data);
            encryptedData = rsa.Encrypt(dataToEncryptBytes, false);
        }
        return new BaseResponse<byte[]>(true,"Encryptd SUccessfully",encryptedData);
    }

    public BaseResponse<EncryptionEntity> GenerateEncryptionKey()
    {
        using (var rsa = new RSACryptoServiceProvider(2048)) // 2048-bit key size
        {
            var publicKey = rsa.ExportParameters(false);
            var privateKey = rsa.ExportParameters(true);

            var response = new EncryptionEntity();
            if (privateKey.Modulus == null || publicKey.Modulus == null || publicKey.Exponent == null || privateKey.Exponent == null)
            {
                return new BaseResponse<EncryptionEntity>
                {
                    Status = false,
                    Message = "Error generating key pair: Modulus or Exponent is null"
                };
            }
            var keyPair = new EncryptionEntity
            {
                PublicKeyModulus = Convert.ToBase64String(publicKey.Modulus),
                PublicKeyExponent = Convert.ToBase64String(publicKey.Exponent),
                PrivateKeyModulus = Convert.ToBase64String(privateKey.Modulus),
                PrivateKeyExponent = Convert.ToBase64String(privateKey.Exponent),
                privatersa = privateKey,
                publicrsa = publicKey
                // Assign other components of the private key as needed
            };

            return new BaseResponse<EncryptionEntity>(true, "Fetched", keyPair);
        }
    }
}
public class RsaKeyPair
{
    public string PublicKeyModulus { get; set; } = default!;
    public string PublicKeyExponent { get; set; } = default!;
    public string PrivateKeyModulus { get; set; } = default!;

    public string PrivateKeyExponent { get; set; } = default!;
}
    // Include other components of the private key as needed

