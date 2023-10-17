using Microsoft.AspNetCore.Identity;
using System.Text;
using TTBack.Models;
using System.Security.Cryptography;

public class MyPasswordHasher : IPasswordHasher<User>
{
    public string HashPassword(User user, string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Преобразуйте хешированные байты в строку HEX
            string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            return hashedPassword;
        }
    }

    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        if (hashedPassword.Equals(HashPassword(user, providedPassword), StringComparison.OrdinalIgnoreCase))
        {
            return PasswordVerificationResult.Success;
        }

        return PasswordVerificationResult.Failed;

    }
}
