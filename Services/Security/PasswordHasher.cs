using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SimpleFreeBoard.Services.Security;

public class PasswordHasher
{
    public byte[] ComputeHash(String password, byte[] salt) => KeyDerivation.Pbkdf2(
        password: password,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA512,
        iterationCount: 10000,
        numBytesRequested: 16);

    public string HashPassword(string password)
    {
        byte[] salt = new byte[16];
        
        RandomNumberGenerator.Create().GetBytes(salt);

        byte[] hash = ComputeHash(password, salt);
        
        return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
    }

    public bool VerifyPassword(string passwordHash, string providedPassword)
    {
        string[] parts = passwordHash.Split(":");
        if (parts.Length != 2)
            return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] originalHash = Convert.FromBase64String(parts[1]);
        
        return ComputeHash(providedPassword, salt)
            .SequenceEqual(originalHash);
    }
}