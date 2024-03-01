using System.Security.Cryptography;

namespace Core.UnitTests.Shared;



public static class RandomStringGenerator
{
    private static readonly Random random = new();

    public static string GenerateUnsecure(int length, bool numeric = false)
    {
        string chars = numeric ? "0123456789" : "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static string Generate(int length)
    {
        var rng = RandomNumberGenerator.Create();
        byte[] rno = new byte[length];
        rng.GetBytes(rno);
        return Convert.ToBase64String(rno).Replace("/", string.Empty).Replace(" ", string.Empty)[..length];
    }
}