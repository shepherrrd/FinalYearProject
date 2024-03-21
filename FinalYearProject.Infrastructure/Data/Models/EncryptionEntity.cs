

using System.Security.Cryptography;

namespace FinalYearProject.Infrastructure.Data.Models;

public class EncryptionEntity
{
    public string PublicKeyModulus { get; set; } = default!;
    public string PublicKeyExponent { get; set; } = default!;
    public string PrivateKeyModulus { get; set; } = default!;

    public string PrivateKeyExponent { get; set; } = default!;
    public RSAParameters publicrsa { get; set; }
    public RSAParameters privatersa { get; set; }
}
