using System.Security.Cryptography;
using AutoMapper.Internal.Mappers;

namespace relojChecadorAPI;

public interface HashInterface
{
    string Hash(string input);
    bool Verify(string input, string hash);
}
public class HashingService :IHashingService
{
    private readonly string _pepper;

    public HashingService(IConfiguration config)
    {
        _pepper = config["Security:Pepper"]!;
    }

    public string Hash(string input)
    {
        var combined = input + _pepper;

        byte[] salt = RandomNumberGenerator.GetBytes(16);

        var pbkdf2 = new Rfc2898DeriveBytes(combined, salt, 100000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);

        return Convert.ToBase64String(salt.Concat(hash).ToArray());
    }

    public bool Verify(string input, string storedHash)
    {
        var fullHashBytes = Convert.FromBase64String(storedHash);

        byte[] salt = fullHashBytes.Take(16).ToArray();
        byte[] hash = fullHashBytes.Skip(16).ToArray();

        var combined = input + _pepper;

        var pbkdf2 = new Rfc2898DeriveBytes(combined, salt, 100000, HashAlgorithmName.SHA256);
        byte[] newHash = pbkdf2.GetBytes(32);

        return hash.SequenceEqual(newHash);
    }
}
