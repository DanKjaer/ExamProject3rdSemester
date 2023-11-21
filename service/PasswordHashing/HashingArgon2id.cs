using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace service.PasswordHashing;

public class HashingArgon2id
{
    public string HashPassword(string password, string salt)
    {
        using var hashAlgo = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = Decode(salt),
            MemorySize = 12288,
            Iterations = 3,
            DegreeOfParallelism = 1,
        };
        return Encode(hashAlgo.GetBytes(256));
    }

    public bool VerifyHashedPassword(string password, string hash, string salt)
    {
        return HashPassword(password, salt).SequenceEqual(hash);
    }

    private byte[] Decode(string value)
    {
        return Convert.FromBase64String(value);
    }

    private string Encode(byte[] value)
    {
        return Convert.ToBase64String(value);
    }
    
    public string GenerateSalt()
    {
        return Encode(RandomNumberGenerator.GetBytes(128));
    }
}